using Blog.Repository;
using Blog.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Blog.Domain;

namespace Timer
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer;

        public IHistoryRepository HistoryRepository;
        public Service1()
        {
            HistoryRepository = new HistoryRepository();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(ClearGarbage);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }

        private void ClearGarbage(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (false)
            {
                HistoryRepository.DeleteAll();
            }
        }
    }
}
