﻿namespace NServiceBus.AcceptanceTests.BestPractices
{
    using System;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NUnit.Framework;

    public class When_unsubscribing_to_command : NServiceBusAcceptanceTest
    {
        [Test]
        public void Should_throw()
        {
            var context = new Context();
            
            Scenario.Define(context)
                    .WithEndpoint<Endpoint>(b => b.Given((bus, c) =>
                    {
                        try
                        {
                            bus.Unsubscribe<MyCommand>();
                        }
                        catch (Exception ex)
                        {
                            c.Exception = ex;
                            c.GotTheException = true;
                        }
                    }))
                    .Done(c => c.GotTheException)
                    .AllowExceptions()
                    .Run();

            Assert.IsInstanceOf<Exception>(context.Exception);
        }

        public class Context : ScenarioContext
        {
            public bool GotTheException { get; set; }
            public Exception Exception{ get; set; }
        }

        public class Endpoint : EndpointConfigurationBuilder
        {
            public Endpoint()
            {
                EndpointSetup<DefaultServer>();
            }
        }
        public class MyCommand : ICommand{}
    }
}
