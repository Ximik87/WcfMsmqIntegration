﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Msmq.Wcf.ConsoleClientLoad.WcfService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfService.ITestContract")]
    public interface ITestContract {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ITestContract/Create")]
        void Create(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/ITestContract/Create")]
        System.Threading.Tasks.Task CreateAsync(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITestContractChannel : Msmq.Wcf.ConsoleClientLoad.WcfService.ITestContract, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TestContractClient : System.ServiceModel.ClientBase<Msmq.Wcf.ConsoleClientLoad.WcfService.ITestContract>, Msmq.Wcf.ConsoleClientLoad.WcfService.ITestContract {
        
        public TestContractClient() {
        }
        
        public TestContractClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TestContractClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TestContractClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TestContractClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Create(string name) {
            base.Channel.Create(name);
        }
        
        public System.Threading.Tasks.Task CreateAsync(string name) {
            return base.Channel.CreateAsync(name);
        }
    }
}
