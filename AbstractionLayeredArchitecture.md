
# Abstraction Layered Architecture

**A brief description**
[(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com)


## The two fundamental constraints of ALA

In ALA, the only unit of code is an abstraction, which is different from a module or class.
ALA has two fundamental constraints for these abstractions. 

1. Dependencies must be on abstractions that are more abstract.
2. Abstractions must be small (no larger than around 500 LOC or equivalent)

## Dependencies and coupling

ALA distinquishes between good and bad dependencies. A dependency on a more abstract abstraction is a good dependency becasue it has no coupling between the code that uses the abstraction and the code that implements the abstraction. It is also good becasue we are reusing the abstraction. As Charles Krueger said, abstraction and reuse are two sides of the same coin. We want many of these types of dependencies in our software, the more the better.

There are at least two types of bad dependencies. One is a dependency for getting data or communication between different modules of a program. The other is when one module is broken up arbitrarily into smaller modules. These smaller modules will tend to be more specific than their containing module, becasue they will be doing a specific job for it. Both these types of dependencies will introduce coupling in the form of collaboration between modules. In ALA both these dependencies are illegal.

Becasue ALA uses only good dependecies, it features zero coupling. By coupling we mean any type of explicit or implicit collaboration betwen code. You may be thinking that collaboration or coupling is essential to making modular systems work. This is a false meme. In ALA, all collaboration or cupling that would have been between modules in conventional code is completely contained inside a new abstraction in a higher layer. There it becomes cohesion.

We don't worry about the structure inside an abstraction. It is meant to be cohesive anyway. So there will be references everywhere between methods, private variables, ports, enums and even at times two small classes. But when the code inside one abstraction uses another abstraction, we observe the dependency ALA rule.

## Layers

The dependency rule cuases the emergence of abstraction layers. Abstraction layers are different from the normal layers in a layered architecture or whats often referred to these days as a stack. Such stacks in ALA be instances of abstarctions in teh same wired togther by an abstraction in a higher layer. 

ALA layers will have abstarctions that tend to have characteristic, so we name the layers after these characteristics as follows. 


### Application layer

The application layer and folder is the top layer. It typically contains code that formally expresses all the user stories and other details of requirements, but no implementation. It is often in the form of a diagram, simply because of the many cohesive relationships implicit in requirements. All implementation is inside *domain abstractions*, each of which provides a conceptual piece of functionality. The application works by configuring and composing instances of domain abstractions.

### Domain abstractions layer

The domain abstractions layer and folder is the second layer. Domain abstractions must be more abstract (and therefore more reusable) than the specific application. We even use multiple instances of some of them in the application. Domain abstractions are usually implemented as classes, but may be functions. In ALA it is ok for abstraction instances to have state.

### Programming paradigms layer

The programming paradigms layer and folder is the third layer. Programming paradigms must be even more abstract (and therefore even more reusable) than domain abstractions. They are called programming paradigms becasue at this level of abstractions they are setting in place patterns for how we will use domain abstarctions to create programs. Programming paradigms will often encompass an *execution model*, which controls how the programming paradigm actullay works in terms of its execution by the CPU.

### Libraries layer

The libraries layer and folder is the bottom layer. It contains generic and widely reusable abstractions.

## Wiring pattern

One way to conform to ALA is to use the follwing wiring pattern.

In this pattern, domain abstarctions are implemented as *classes with ports*. Ports are instances programming paradigms and are implemented as an interface. Instances of domain abstractions can then be wired together by the application if they have compatible ports.

Say you have two domain abstractions A and B, which you know can be wired together because they have complementary ports of the event-driven programming paradigm.

We can then write the application layer code:

```
using DomainAbstractions;
using ProgrammingParadigms;
using Libraries;

namespace Application
{
    public class Application
    {
        public Application()
        {
            new A().WireTo(new B()).Run();
        }
    }
}
```
    
This code will instantiate an A and B and wire them together.
The WireTo method is an extension method implemented in the libraries layer.
Let's say the event-driven programming paradigm we are using is a simple synchronous execution model and is implemented by IEvent:


```
namespace ProgrammingParadigms
{
    public interface IEvent
    {
        void Execute();
    }
}
```


Now the code for our two classes will have complementary ports of that programming paradigm as follows:


```
using ProgrammingParadigms;
namespace DomainAbstractions
{
    class A
    {
        private IEvent output;
        public void Run()
        {
            output?.Execute();    
        }
    }
}
```


```
using ProgrammingParadigms;
namespace DomainAbstractions
{
    class B : IEvent
    {
        IDataflow.Execute()
        { 
            Console.WriteLine("Event received");
        }
    }
}
```


The WireTo operator is an extension method on all objects. It uses reflection to find a private field in A that has the same interface type as an interface implemented by B. If it finds such an interface, it casts the B to the interface type and assigns it to the field in A. You can see that way ports are done depends on knowledge of the WireTo abstraction that will look for them. 

WireTo has an optional second parameter which is the name of the port in A. This is used when we need to ensure that the correct port is wired.
A port can also be a list of interfaces to support fanout. The field in A is private because we don't want anything else to see it - only the WireTo operator.

This wiring pattern is one way to conform to the constraints provided by the fundamental rules of ALA, but we use it extensively.

## Conclusion

Now that you understand the fundamental principles of ALA, and the emergent layers and wiring pattern, you will be able to understand the ALAexample application, why it is organised into the folders it has, and how the whole things actually executes.

There is a bit more to the philosophy and design behind the ALA architecture. Further details can be found in the Introduction and Chapter 2 of the web site <http://www.abstractionlayeredarchitecture.com>.



