using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateChains
{
    delegate void Notify(string message);

    class Notifier
    {
        public Notify EventOccured;
    }

    class EventListener
    {
        private string name;
        public EventListener(string name)
        {
            this.name = name;
        }

        public void SomethingHappend(string message)
        {
            Console.WriteLine($"{name}.SomthingHappend : {message}");
        }
    }

    class MainApp
    {
        static void Main(string[] args)
        {
            Notifier notifier = new Notifier();
            EventListener listener1 = new EventListener("Listener1");
            EventListener listener2 = new EventListener("Listener2");
            EventListener listener3 = new EventListener("Listener3");

            // += 이용하여 체인 만들기
            notifier.EventOccured += listener1.SomethingHappend;
            notifier.EventOccured += listener2.SomethingHappend;
            notifier.EventOccured += listener3.SomethingHappend;
            notifier.EventOccured("You've got mail");

            Console.WriteLine();

            // -= 이용하여 체인 끊기
            notifier.EventOccured -= listener2.SomethingHappend;
            notifier.EventOccured("Download complete");

            Console.WriteLine();

            // +, = 연산자로 체인 만들기
            notifier.EventOccured = new Notify(listener2.SomethingHappend) + new Notify(listener3.SomethingHappend);
            notifier.EventOccured("Nuclear launch detected");

            Console.WriteLine();

            /* Result
                Listener1.SomthingHappend : You've got mail
                Listener2.SomthingHappend : You've got mail
                Listener3.SomthingHappend : You've got mail

                Listener1.SomthingHappend : Download complete
                Listener3.SomthingHappend : Download complete

                Listener2.SomthingHappend : Nuclear launch detected
                Listener3.SomthingHappend : Nuclear launch detected
            */
        }
    }
}
