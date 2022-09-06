using System;
using MaterializeExcel.Events;
using MaterializeExcelAddIn.Manipulation;
using NLog;
using ReactiveUI;

namespace MaterializeExcelAddIn.Controller
{
    public class WpfInteractionController : IDisposable
    {
        // private readonly IEventAggregator eventAgg;
        private readonly ExcelInteraction _excelInteraction;
        // private readonly SubscriptionToken tokenMeetingData;
        private readonly IDisposable _addToSheetSubscription;

        public WpfInteractionController(ExcelInteraction excelInteraction)
        {
            // this.eventAgg = eventAgg;
            _excelInteraction = excelInteraction;

            _addToSheetSubscription = MessageBus.Current.Listen<AddToSheetRequest>()
                // .Where(e => e.KeyCode == KeyCode.Up)
                .Subscribe(x => Console.WriteLine(x.ObjectName));
            
            // tokenMeetingData = eventAgg.GetEvent<PubSubEvent<ExcelMeetingDataRequest>>()
            //     .Subscribe(WriteMeetingData);
            //
            // tokenSheetName = eventAgg.GetEvent<PubSubEvent<ExcelWorksheetNamesRequest>>()
            //     .Subscribe(GetWorksheetsName);
        }

        // private void GetWorksheetsName(ExcelWorksheetNamesRequest obj)
        // {
        //     logger.Debug("Return excel sheets names");
        //
        //     var response = new ExcelWorksheetNamesResponse
        //     {
        //         SheetNames = excelOperation.WorksheetsName().ToArray(),
        //     };
        //
        //     eventAgg.GetEvent<PubSubEvent<ExcelWorksheetNamesResponse>>()
        //         .Publish(response);
        // }
        //
        private void WriteMeetingData(AddToSheetRequest request)
        {
            // logger.Debug("Write Meeting data");
            try
            {
                _excelInteraction.WriteQueryToSheet(request);
                // eventAgg.GetEvent<PubSubEvent<ExcelMeetingDataResponse>>()
                    // .Publish(new ExcelMeetingDataResponse
                    // { ProcessCompletedSuccessfully = true });
            }
            catch (Exception)
            {
                // eventAgg.GetEvent<PubSubEvent<ExcelMeetingDataResponse>>()
                    // .Publish(new ExcelMeetingDataResponse
                    // { ProcessCompletedSuccessfully = false });
            }
        
        }

        public void Dispose()
        {
            _addToSheetSubscription.Dispose();
            
            // eventAgg.GetEvent<PubSubEvent<ExcelMeetingDataRequest>>()
            //     .Unsubscribe(tokenMeetingData);
            //
            // eventAgg.GetEvent<PubSubEvent<ExcelWorksheetNamesRequest>>()
            //     .Unsubscribe(tokenSheetName);
        }
    }
}