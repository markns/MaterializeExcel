using System;
using System.Reactive.Linq;
using MaterializeExcel.AddIn.Manipulation;
using MaterializeExcel.Events;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.AddIn.Controller
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WpfInteractionController : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ExcelInteraction _excelInteraction;
        private readonly IDisposable _addToSheetSubscription;

        public WpfInteractionController(ExcelInteraction excelInteraction, IMessageBus messageBus)
        {
            _excelInteraction = excelInteraction;
            _addToSheetSubscription = messageBus.Listen<AddToSheetRequest>()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(WriteRequestToSheet);
        }

        private void WriteRequestToSheet(AddToSheetRequest request)
        {
            try
            {
                _excelInteraction.WriteQueryToSheet(request);
            }
            catch (Exception e)
            {
                // TODO: return event with failure details, and use UserControlExtensions to pop up warning.
                Logger.Error($"{e.Message} {e.StackTrace}");
                // eventAgg.GetEvent<PubSubEvent<ExcelMeetingDataResponse>>()
                // .Publish(new ExcelMeetingDataResponse
                // { ProcessCompletedSuccessfully = false });
                throw;
            }
        }

        public void Dispose()
        {
            _addToSheetSubscription.Dispose();
        }
    }
}