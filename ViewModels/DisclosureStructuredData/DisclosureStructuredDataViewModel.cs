using IS_Proj_HIT.Entities;

namespace IS_Proj_HIT.ViewModels.DisclosureStructuredData
{
    public class DisclosureStructuredDataViewModel
    {
        public DocumentRequested DocumentRequested {get; set;}
        public RequestPriority RequestPriority {get; set;}
        public RequestPurpose RequestPurpose { get; set;}
        public RequestReleaseFormat RequestReleaseFormat{get; set;}
        public RequestStatus RequestStatus {get; set;}
        public RequestStatusReason RequestStatusReason {get; set;}
        public ItemStatus ItemStatus {get; set;}
        public RequesterStatus RequesterStatus {get; set;}
        public RequesterType RequesterType {get; set;}

        // General constructors
        public DisclosureStructuredDataViewModel()
        {}

        public DisclosureStructuredDataViewModel(DocumentRequested documentRequested){
            this.DocumentRequested = documentRequested;
        }

        public DisclosureStructuredDataViewModel(RequestPriority requestPriority){
            this.RequestPriority = requestPriority;
        }

        public DisclosureStructuredDataViewModel(RequestPurpose requestPurpose){
            this.RequestPurpose = requestPurpose;
        }
        public DisclosureStructuredDataViewModel(RequestReleaseFormat requestReleaseFormat){
            this.RequestReleaseFormat = requestReleaseFormat;
        }

        public DisclosureStructuredDataViewModel(RequestStatus requestStatus){
            this.RequestStatus = requestStatus;
        }

        public DisclosureStructuredDataViewModel(RequestStatusReason requestStatusReason){
            this.RequestStatusReason = requestStatusReason;
        }

        public DisclosureStructuredDataViewModel(ItemStatus itemStatus){
            this.ItemStatus = itemStatus;
        }

        public DisclosureStructuredDataViewModel(RequesterStatus requesterStatus){
            this.RequesterStatus = requesterStatus;
        }

        public DisclosureStructuredDataViewModel(RequesterType requesterType){
            this.RequesterType = requesterType;
        }


    }
}