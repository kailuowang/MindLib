using System;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MindHarbor.Scheduler;

/// <summary>
/// A mock task to test AsynchornizedProgress
/// </summary>
/// <remarks>
/// it tooks 30 secs to finish
/// IInterruptable is optional
/// </remarks>
public class MockTask : AsynchronizedProcessTask , IInterruptable
{
    private int percentageProgress;
    private bool interrupted = false;

    #region IInterruptable Members

    public void Interrupt() {
        interrupted = true;
    }

    #endregion


    public MockTask(string name) :base (name)
    {}

    public override float PercentageProgress {
        get { return percentageProgress; }
    }

    protected override void Perform() {
        AddLog("Mock task is startted");
        for (int i = 0; i < 10; i++)
        {
            if(interrupted) {
                AddLog("Mock task is interrupted");
                break;
            }
            percentageProgress = i*100/30;
            AddLog("Mock task proceeded to number " + (i + 1));
           
            //let the thread sleep to pretend doing some heavy load process
            Thread.Sleep(500);
        }
        AddLog("Mock task is Done!");
    }
}
