
# Abstraction Layered Architecture brief description

[(Abstraction Layered Architecture)](http://www.abstractionlayeredarchitecture.com)

In ALA, the only unit of code is an abstraction. Dependencies must be on abstractions that are more abstract. This gives rise to abstraction layers as follows:

### Application layer

The application layer and folder is the top layer. It typically contains code that formally expresses all the user stories and other details of requirements, but no implementation. It is typically in the form of a diagram, simply because of the many relationships implicit in requirements. Implementation is inside domain abstractions, each of which provides a conceptual piece of functionality. The application works by instantiating, configuring and wiring together a composition of domain abstractions.

### Domain abstractions layer:

The domain abstractions layer and folder is the second layer. Domain abstractions must be more abstract (and therefore more reusable) than the specific application. We even use multiple instances of some of them in the application. Domain abstractions are usually implemented as classes with *ports*. Ports allow instances of domain abstractions to be wired together.

### Programming paradigms layer

The programming paradigms layer and folder is the third layer. Programming paradigms must be even more abstract (and therefore even more reusable) than domain abstractions. Ports  are instances of programming paradigms. Different wirings in the diagrams can have different meanings which is why they are called programming paradigms. Programming paradigms are often implemented as interfaces, which allows the ports to be wired by the diagram.

### Libraries layer

The libraries layer and folder is the bottom layer. It contains the *WireTo* extension method used by Application.cs to implement each wiring in application-diagram. WireTo supports this whole pattern of expressing user stories through instances of domain abstractions wired together using programming paradigms. Here is the actual mechanics:

### Code mechanics

Say you have two domain abstractions A and B, which you know can be wired together because they have complementary ports of the event-driven programming paradigm.

We can then write the application layer code:

---
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
---
    
This code will instantiate an A and B and wire them together.
Let's say the event-driven programming paradigm we are wiring with is implemented by IEvent:


---
namespace ProgrammingParadigms
{
    public interface IEvent
    {
        void Execute();
    }
}
---


Now the code for our two classes will have complementary ports of that programming paradigm as follows:


---
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
---


---
using ProgrammingParadigms;
namespace DomainAbstractions
{
    class B : IEvent
    {
        IDataflow.Execute()
        { 
            Console.WriteLine(value);
        }
    }
}
---


The WireTo operator is an extension method on all objects. It uses reflection to find a private field in A that has the same interface type as an interface implemented by B. If it find such an interface, it casts the B to the interface type and assigns that to the field in A. 

WireTo has an optional second parameter which is the name of the port in A. This is used when we needto ensure that the correct port is wired.

The field in A is private because we don't want anything else to see it - only the WireTo operator.

This pattern is one way to conform to the constraints provided by the fundamental rules of ALA.

### Dependencies and coupling in ALA

There are no dependencies within layers, so all abstractions are like standalone programs given knowledge of the abstractions they use. Through the use of abstraction, the internals of all abstractions are zero-coupled in ALA, even going down the layers.

There is a bit more to the philosophy and design of the ALA architecture


Knowledge of ALA itself is needed to understand the architecture of the code. Further details can be found in the Introduction and Chapter 2 of the web site <http://www.abstractionlayeredarchitecture.com>.



