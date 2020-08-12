using System;
using System.Collections.Generic;
using System.Text;

namespace WeeklyXamarin.Core.Services
{
    public enum Activity
    {
        Follow,
        Subscribe,
        Raid,
        PullRequest
    }

    public class Acknowledgement
    {
        public string Person { get; set; }
        public Activity Activity { get; set; }
    }

    public class Acknowledgements
    {
        public IList<Acknowledgement> Thanks = new List<Acknowledgement>();

        public Acknowledgements()
        {
            Thanks = new List<Acknowledgement>()
            {
                // 5 August 2020
                new Acknowledgement() {Person="@riallymundane", Activity=Activity.Raid},
                new Acknowledgement() {Person="@Superbeer1994", Activity=Activity.Follow},
                new Acknowledgement() {Person="@informationrocket", Activity=Activity.Follow},
                new Acknowledgement() {Person="@STuaRTRuLes", Activity=Activity.Follow},
                new Acknowledgement() {Person="@StevenThewissen", Activity=Activity.Follow},
                new Acknowledgement() {Person="@fr33k3r", Activity=Activity.Follow},
                new Acknowledgement() {Person="@DavidWengier", Activity=Activity.Raid},
                new Acknowledgement() {Person="@deva73", Activity=Activity.Follow},
                new Acknowledgement() {Person="@pickpie", Activity=Activity.Follow},
                new Acknowledgement() {Person="@vishop_vishwa", Activity=Activity.Follow},
                new Acknowledgement() {Person="@codingwithLuce", Activity=Activity.PullRequest},
            };
        }


    }
}
