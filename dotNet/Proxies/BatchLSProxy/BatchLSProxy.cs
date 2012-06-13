﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.42.
// 
namespace iLabs.Proxies.BatchLS {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;

    using iLabs.DataTypes.BatchTypes;
    using iLabs.ServiceBroker.Services;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="I_BLS", Namespace="http://ilab.mit.edu")]
    public partial class BatchLSProxy : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        public AuthHeader AuthHeaderValue;
        
        private System.Threading.SendOrPostCallback CancelOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLabStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetEffectiveQueueLengthOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLabInfoOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetLabConfigurationOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateOperationCompleted;
        
        private System.Threading.SendOrPostCallback SubmitOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetExperimentStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrieveResultOperationCompleted;
        
        /// <remarks/>
        public BatchLSProxy() {
            this.Url = "http://localhost/ilab_WSDL/I_BLS.asmx";
        }
        
        /// <remarks/>
        public event CancelCompletedEventHandler CancelCompleted;
        
        /// <remarks/>
        public event GetLabStatusCompletedEventHandler GetLabStatusCompleted;
        
        /// <remarks/>
        public event GetEffectiveQueueLengthCompletedEventHandler GetEffectiveQueueLengthCompleted;
        
        /// <remarks/>
        public event GetLabInfoCompletedEventHandler GetLabInfoCompleted;
        
        /// <remarks/>
        public event GetLabConfigurationCompletedEventHandler GetLabConfigurationCompleted;
        
        /// <remarks/>
        public event ValidateCompletedEventHandler ValidateCompleted;
        
        /// <remarks/>
        public event SubmitCompletedEventHandler SubmitCompleted;
        
        /// <remarks/>
        public event GetExperimentStatusCompletedEventHandler GetExperimentStatusCompleted;
        
        /// <remarks/>
        public event RetrieveResultCompletedEventHandler RetrieveResultCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/Cancel", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool Cancel(int experimentID) {
            object[] results = this.Invoke("Cancel", new object[] {
                        experimentID});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginCancel(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Cancel", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndCancel(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void CancelAsync(int experimentID) {
            this.CancelAsync(experimentID, null);
        }
        
        /// <remarks/>
        public void CancelAsync(int experimentID, object userState) {
            if ((this.CancelOperationCompleted == null)) {
                this.CancelOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCancelOperationCompleted);
            }
            this.InvokeAsync("Cancel", new object[] {
                        experimentID}, this.CancelOperationCompleted, userState);
        }
        
        private void OnCancelOperationCompleted(object arg) {
            if ((this.CancelCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CancelCompleted(this, new CancelCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/GetLabStatus", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public LabStatus GetLabStatus() {
            object[] results = this.Invoke("GetLabStatus", new object[0]);
            return ((LabStatus)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetLabStatus(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetLabStatus", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public LabStatus EndGetLabStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((LabStatus)(results[0]));
        }
        
        /// <remarks/>
        public void GetLabStatusAsync() {
            this.GetLabStatusAsync(null);
        }
        
        /// <remarks/>
        public void GetLabStatusAsync(object userState) {
            if ((this.GetLabStatusOperationCompleted == null)) {
                this.GetLabStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLabStatusOperationCompleted);
            }
            this.InvokeAsync("GetLabStatus", new object[0], this.GetLabStatusOperationCompleted, userState);
        }
        
        private void OnGetLabStatusOperationCompleted(object arg) {
            if ((this.GetLabStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLabStatusCompleted(this, new GetLabStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/GetEffectiveQueueLength", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public WaitEstimate GetEffectiveQueueLength(string userGroup, int priorityHint) {
            object[] results = this.Invoke("GetEffectiveQueueLength", new object[] {
                        userGroup,
                        priorityHint});
            return ((WaitEstimate)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetEffectiveQueueLength(string userGroup, int priorityHint, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetEffectiveQueueLength", new object[] {
                        userGroup,
                        priorityHint}, callback, asyncState);
        }
        
        /// <remarks/>
        public WaitEstimate EndGetEffectiveQueueLength(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((WaitEstimate)(results[0]));
        }
        
        /// <remarks/>
        public void GetEffectiveQueueLengthAsync(string userGroup, int priorityHint) {
            this.GetEffectiveQueueLengthAsync(userGroup, priorityHint, null);
        }
        
        /// <remarks/>
        public void GetEffectiveQueueLengthAsync(string userGroup, int priorityHint, object userState) {
            if ((this.GetEffectiveQueueLengthOperationCompleted == null)) {
                this.GetEffectiveQueueLengthOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetEffectiveQueueLengthOperationCompleted);
            }
            this.InvokeAsync("GetEffectiveQueueLength", new object[] {
                        userGroup,
                        priorityHint}, this.GetEffectiveQueueLengthOperationCompleted, userState);
        }
        
        private void OnGetEffectiveQueueLengthOperationCompleted(object arg) {
            if ((this.GetEffectiveQueueLengthCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetEffectiveQueueLengthCompleted(this, new GetEffectiveQueueLengthCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/GetLabInfo", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetLabInfo() {
            object[] results = this.Invoke("GetLabInfo", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetLabInfo(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetLabInfo", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetLabInfo(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetLabInfoAsync() {
            this.GetLabInfoAsync(null);
        }
        
        /// <remarks/>
        public void GetLabInfoAsync(object userState) {
            if ((this.GetLabInfoOperationCompleted == null)) {
                this.GetLabInfoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLabInfoOperationCompleted);
            }
            this.InvokeAsync("GetLabInfo", new object[0], this.GetLabInfoOperationCompleted, userState);
        }
        
        private void OnGetLabInfoOperationCompleted(object arg) {
            if ((this.GetLabInfoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLabInfoCompleted(this, new GetLabInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/GetLabConfiguration", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetLabConfiguration(string userGroup) {
            object[] results = this.Invoke("GetLabConfiguration", new object[] {
                        userGroup});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetLabConfiguration(string userGroup, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetLabConfiguration", new object[] {
                        userGroup}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetLabConfiguration(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetLabConfigurationAsync(string userGroup) {
            this.GetLabConfigurationAsync(userGroup, null);
        }
        
        /// <remarks/>
        public void GetLabConfigurationAsync(string userGroup, object userState) {
            if ((this.GetLabConfigurationOperationCompleted == null)) {
                this.GetLabConfigurationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetLabConfigurationOperationCompleted);
            }
            this.InvokeAsync("GetLabConfiguration", new object[] {
                        userGroup}, this.GetLabConfigurationOperationCompleted, userState);
        }
        
        private void OnGetLabConfigurationOperationCompleted(object arg) {
            if ((this.GetLabConfigurationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetLabConfigurationCompleted(this, new GetLabConfigurationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/Validate", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ValidationReport Validate(string experimentSpecification, string userGroup) {
            object[] results = this.Invoke("Validate", new object[] {
                        experimentSpecification,
                        userGroup});
            return ((ValidationReport)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginValidate(string experimentSpecification, string userGroup, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Validate", new object[] {
                        experimentSpecification,
                        userGroup}, callback, asyncState);
        }
        
        /// <remarks/>
        public ValidationReport EndValidate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ValidationReport)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateAsync(string experimentSpecification, string userGroup) {
            this.ValidateAsync(experimentSpecification, userGroup, null);
        }
        
        /// <remarks/>
        public void ValidateAsync(string experimentSpecification, string userGroup, object userState) {
            if ((this.ValidateOperationCompleted == null)) {
                this.ValidateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateOperationCompleted);
            }
            this.InvokeAsync("Validate", new object[] {
                        experimentSpecification,
                        userGroup}, this.ValidateOperationCompleted, userState);
        }
        
        private void OnValidateOperationCompleted(object arg) {
            if ((this.ValidateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateCompleted(this, new ValidateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/Submit", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SubmissionReport Submit(int experimentID, string experimentSpecification, string userGroup, int priorityHint) {
            object[] results = this.Invoke("Submit", new object[] {
                        experimentID,
                        experimentSpecification,
                        userGroup,
                        priorityHint});
            return ((SubmissionReport)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSubmit(int experimentID, string experimentSpecification, string userGroup, int priorityHint, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Submit", new object[] {
                        experimentID,
                        experimentSpecification,
                        userGroup,
                        priorityHint}, callback, asyncState);
        }
        
        /// <remarks/>
        public SubmissionReport EndSubmit(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((SubmissionReport)(results[0]));
        }
        
        /// <remarks/>
        public void SubmitAsync(int experimentID, string experimentSpecification, string userGroup, int priorityHint) {
            this.SubmitAsync(experimentID, experimentSpecification, userGroup, priorityHint, null);
        }
        
        /// <remarks/>
        public void SubmitAsync(int experimentID, string experimentSpecification, string userGroup, int priorityHint, object userState) {
            if ((this.SubmitOperationCompleted == null)) {
                this.SubmitOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubmitOperationCompleted);
            }
            this.InvokeAsync("Submit", new object[] {
                        experimentID,
                        experimentSpecification,
                        userGroup,
                        priorityHint}, this.SubmitOperationCompleted, userState);
        }
        
        private void OnSubmitOperationCompleted(object arg) {
            if ((this.SubmitCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubmitCompleted(this, new SubmitCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/GetExperimentStatus", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public LabExperimentStatus GetExperimentStatus(int experimentID) {
            object[] results = this.Invoke("GetExperimentStatus", new object[] {
                        experimentID});
            return ((LabExperimentStatus)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetExperimentStatus(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetExperimentStatus", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public LabExperimentStatus EndGetExperimentStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((LabExperimentStatus)(results[0]));
        }
        
        /// <remarks/>
        public void GetExperimentStatusAsync(int experimentID) {
            this.GetExperimentStatusAsync(experimentID, null);
        }
        
        /// <remarks/>
        public void GetExperimentStatusAsync(int experimentID, object userState) {
            if ((this.GetExperimentStatusOperationCompleted == null)) {
                this.GetExperimentStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetExperimentStatusOperationCompleted);
            }
            this.InvokeAsync("GetExperimentStatus", new object[] {
                        experimentID}, this.GetExperimentStatusOperationCompleted, userState);
        }
        
        private void OnGetExperimentStatusOperationCompleted(object arg) {
            if ((this.GetExperimentStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetExperimentStatusCompleted(this, new GetExperimentStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/RetrieveResult", RequestNamespace="http://ilab.mit.edu", ResponseNamespace="http://ilab.mit.edu", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ResultReport RetrieveResult(int experimentID) {
            object[] results = this.Invoke("RetrieveResult", new object[] {
                        experimentID});
            return ((ResultReport)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRetrieveResult(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveResult", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public ResultReport EndRetrieveResult(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ResultReport)(results[0]));
        }
        
        /// <remarks/>
        public void RetrieveResultAsync(int experimentID) {
            this.RetrieveResultAsync(experimentID, null);
        }
        
        /// <remarks/>
        public void RetrieveResultAsync(int experimentID, object userState) {
            if ((this.RetrieveResultOperationCompleted == null)) {
                this.RetrieveResultOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrieveResultOperationCompleted);
            }
            this.InvokeAsync("RetrieveResult", new object[] {
                        experimentID}, this.RetrieveResultOperationCompleted, userState);
        }
        
        private void OnRetrieveResultOperationCompleted(object arg) {
            if ((this.RetrieveResultCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrieveResultCompleted(this, new RetrieveResultCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public void CancelAsync1(object userState) {
            base.CancelAsync(userState);
        }
    }
    
  
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void CancelCompletedEventHandler(object sender, CancelCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CancelCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CancelCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetLabStatusCompletedEventHandler(object sender, GetLabStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLabStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLabStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public LabStatus Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((LabStatus)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetEffectiveQueueLengthCompletedEventHandler(object sender, GetEffectiveQueueLengthCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetEffectiveQueueLengthCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetEffectiveQueueLengthCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WaitEstimate Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WaitEstimate)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetLabInfoCompletedEventHandler(object sender, GetLabInfoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLabInfoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLabInfoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetLabConfigurationCompletedEventHandler(object sender, GetLabConfigurationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetLabConfigurationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetLabConfigurationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ValidateCompletedEventHandler(object sender, ValidateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ValidationReport Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ValidationReport)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void SubmitCompletedEventHandler(object sender, SubmitCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubmitCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SubmitCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SubmissionReport Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SubmissionReport)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetExperimentStatusCompletedEventHandler(object sender, GetExperimentStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetExperimentStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetExperimentStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public LabExperimentStatus Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((LabExperimentStatus)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void RetrieveResultCompletedEventHandler(object sender, RetrieveResultCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RetrieveResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrieveResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ResultReport Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ResultReport)(this.results[0]));
            }
        }
    }
}
