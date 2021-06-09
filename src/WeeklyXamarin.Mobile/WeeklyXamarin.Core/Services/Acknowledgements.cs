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
        PullRequest,
        Bitties,
        DerailedEverything,
        GiftSub,
        WriteTheThemeTune,
        SavedTheDay,
        Hygiene,
        Host
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
                // 9 June 2021
                new Acknowledgement() {Person="sheepyabc", Activity=Activity.Follow},
                new Acknowledgement() {Person="AdenEarnshaw", Activity=Activity.Bitties},
                new Acknowledgement() {Person="SergejMolotow", Activity=Activity.Follow},
                new Acknowledgement() {Person="ezteh_tarik", Activity=Activity.Follow},
                
                // 3 June 2021
                new Acknowledgement() {Person="bassicaarons", Activity=Activity.Follow},
                new Acknowledgement() {Person="ybadragon", Activity=Activity.Subscribe},

                // 12 May 2021
                new Acknowledgement() {Person="devkeh", Activity=Activity.Follow},
                new Acknowledgement() {Person="nimit_bhinde", Activity=Activity.Follow},
                new Acknowledgement() {Person="juanmillord", Activity=Activity.Follow},


                // 5 May 2021
                new Acknowledgement() {Person="yettobeknowntome", Activity=Activity.Follow},
                new Acknowledgement() {Person="zmamujee", Activity=Activity.Follow},
                new Acknowledgement() {Person="anthalia26", Activity=Activity.Subscribe},
                new Acknowledgement() {Person="enisn_", Activity=Activity.Follow},

                // 7 April 2021
                new Acknowledgement() {Person="mnehus", Activity=Activity.Follow},
                new Acknowledgement() {Person="azzraell22", Activity=Activity.Follow},
                new Acknowledgement() {Person="Bearom_", Activity=Activity.Follow},
                new Acknowledgement() {Person="pandak74", Activity=Activity.Follow},
                new Acknowledgement() {Person="IeuanTWalker", Activity=Activity.Subscribe },
                new Acknowledgement() {Person="BenBtg", Activity=Activity.Subscribe },

                // 31 March 2021
                new Acknowledgement() {Person="twilightsparkle213", Activity=Activity.Follow },
                new Acknowledgement() {Person="celesteapps", Activity=Activity.Follow },
                new Acknowledgement() {Person="StevenThewissen", Activity=Activity.Follow },
                new Acknowledgement() {Person="dnrmy", Activity=Activity.Follow },

                //10 March 2021
                new Acknowledgement() {Person="CrumbsWasTaken", Activity=Activity.Follow },

                //Iforgot what date this was...
                new Acknowledgement() {Person="TendedDinosaur3", Activity=Activity.Host},


                // 10 Feb 2021
                new Acknowledgement() {Person="ortunado", Activity=Activity.Follow},
                new Acknowledgement() {Person="TendedDinosaur3", Activity=Activity.Raid},
                
                // 23 December 2020
                new Acknowledgement() {Person="CodingWithLuce", Activity=Activity.Subscribe},
                new Acknowledgement() {Person="natiginfo", Activity=Activity.Follow},
                new Acknowledgement() {Person="silentcoder1", Activity=Activity.Follow},
                new Acknowledgement() {Person="mallibone", Activity=Activity.Follow},
                new Acknowledgement() {Person="mallibone", Activity=Activity.Hygiene},
                new Acknowledgement() {Person="Stoyan_gm", Activity=Activity.Follow},
                new Acknowledgement() {Person="tbdgamer", Activity=Activity.Subscribe},

                // 15 December 2020
                new Acknowledgement() {Person="devleifr", Activity=Activity.Follow},
                new Acknowledgement() {Person="lobino68", Activity=Activity.Follow},
                new Acknowledgement() {Person="driftkid60", Activity=Activity.Follow},
                new Acknowledgement() {Person="JoaoMobileDev", Activity=Activity.Follow},
                new Acknowledgement() {Person="whitep4nth3r", Activity=Activity.Follow},
                new Acknowledgement() {Person="DelviDeveloper", Activity=Activity.Follow},


                // new Acknowledgement() {Person="", Activity=Activity.Follow},
                // 25 November 2020
                new Acknowledgement() {Person="kazn_creator", Activity=Activity.Raid},
                new Acknowledgement() {Person="kazn_creator", Activity=Activity.Follow},
                new Acknowledgement() {Person="the_real_merengue", Activity=Activity.Follow},
                new Acknowledgement() {Person="MattH14", Activity=Activity.Follow},
                new Acknowledgement() {Person="renatodeousa00", Activity=Activity.Follow},
                new Acknowledgement() {Person="GokuJax", Activity=Activity.Follow},

                // 11 November 2020
                new Acknowledgement() {Person="lukaspanni", Activity=Activity.Follow},
                new Acknowledgement() {Person="m0rde0n", Activity=Activity.Follow},
                new Acknowledgement() {Person="dreener", Activity=Activity.Follow},
                new Acknowledgement() {Person="Gamedeveloper0", Activity=Activity.Follow},
                new Acknowledgement() {Person="mayyhhem", Activity=Activity.Follow},
                new Acknowledgement() {Person="DerNerd", Activity=Activity.Follow},
                new Acknowledgement() {Person="WillyWutz", Activity=Activity.Follow},
                new Acknowledgement() {Person="alenussgipfele27", Activity=Activity.Follow},
                new Acknowledgement() {Person="Messmeryzed", Activity=Activity.Follow},
                new Acknowledgement() {Person="muerc", Activity=Activity.Follow},
                new Acknowledgement() {Person="noobrunner", Activity=Activity.Follow},
                new Acknowledgement() {Person="l4zy_pigeon", Activity=Activity.Follow},
                new Acknowledgement() {Person="ravenonj", Activity=Activity.Follow},
                new Acknowledgement() {Person="gabse191", Activity=Activity.Follow},
                new Acknowledgement() {Person="ProfAHeil", Activity=Activity.Follow},
                new Acknowledgement() {Person="ProfAHeil", Activity=Activity.Raid},
                new Acknowledgement() {Person="ProfAHeil", Activity=Activity.DerailedEverything},

                new Acknowledgement() {Person="moijjo", Activity=Activity.Follow},
                new Acknowledgement() {Person="BenBtg", Activity=Activity.Subscribe},
                new Acknowledgement() {Person="DavidWengier", Activity=Activity.Raid},
                new Acknowledgement() {Person="correiavictor92", Activity=Activity.Follow},



                // 30 September 2020
                new Acknowledgement() {Person="r2_au", Activity=Activity.DerailedEverything },
                new Acknowledgement() {Person="AbhishekSY", Activity=Activity.Follow},
                new Acknowledgement() {Person="r2_au", Activity=Activity.Bitties},

                // 16 September 2020
                new Acknowledgement() {Person="r2_au", Activity=Activity.SavedTheDay},
                new Acknowledgement() {Person="AdenEarnshaw", Activity=Activity.Bitties},
                new Acknowledgement() {Person="smokinGears", Activity=Activity.Follow},
                new Acknowledgement() {Person="funcdev", Activity=Activity.Follow},
                new Acknowledgement() {Person="Uraitz_Olaizola", Activity=Activity.Follow},
                new Acknowledgement() {Person="jorginhobanana", Activity=Activity.Follow},
                new Acknowledgement() {Person="bigglid", Activity=Activity.Follow},
                new Acknowledgement() {Person="TimBeaudet", Activity=Activity.Raid},
                new Acknowledgement() {Person="TimBeaudet", Activity=Activity.Raid},
                new Acknowledgement() {Person="DavidWengier", Activity=Activity.Raid},
                new Acknowledgement() {Person="KevinQAnderson", Activity=Activity.Bitties},
                new Acknowledgement() {Person="r2_au", Activity=Activity.Bitties},
                new Acknowledgement() {Person="blackclaw404", Activity=Activity.Follow},
                new Acknowledgement() {Person="yuvarajcena26", Activity=Activity.Follow},
                new Acknowledgement() {Person="johandeveloper", Activity=Activity.Follow},
                new Acknowledgement() {Person="mandragoraX", Activity=Activity.Follow},
                new Acknowledgement() {Person="m_2_", Activity=Activity.Follow},

                // 9 September 2020
                new Acknowledgement() {Person="ThisIsDavid", Activity=Activity.Follow},
                new Acknowledgement() {Person="KlausDevWalker", Activity=Activity.Follow},
                new Acknowledgement() {Person="TheXtremePerson", Activity=Activity.Follow},
                new Acknowledgement() {Person="KickInThePocket", Activity=Activity.Follow},
                new Acknowledgement() {Person="FBoucheros", Activity=Activity.Raid},
                new Acknowledgement() {Person="xami3shah", Activity=Activity.Follow},
                new Acknowledgement() {Person="functionjarvis20", Activity=Activity.Follow},
                new Acknowledgement() {Person="Willairways", Activity=Activity.Follow},
                new Acknowledgement() {Person="BjornOveBremmnes", Activity=Activity.Follow},
                new Acknowledgement() {Person="Scratch2k", Activity=Activity.Follow},
                new Acknowledgement() {Person="lubdubw", Activity=Activity.Follow},
                new Acknowledgement() {Person="r2_au", Activity=Activity.Bitties},

                new Acknowledgement() {Person="RMauroDev", Activity=Activity.Follow},
                new Acknowledgement() {Person="SurlyDev", Activity=Activity.Follow},
                new Acknowledgement() {Person="TheGrumpyGameDev", Activity=Activity.Raid},
                new Acknowledgement() {Person="LachlanWGordon", Activity=Activity.Bitties},
                new Acknowledgement() {Person="InquisitorJax", Activity=Activity.Follow},
                new Acknowledgement() {Person="AdenEarnshaw", Activity=Activity.Subscribe},


                // 2 September 2020
                new Acknowledgement() {Person="TendedDinosaur2", Activity=Activity.Follow },
                new Acknowledgement() {Person="AndikaRizary", Activity=Activity.Follow },
                new Acknowledgement() {Person="jazteng2", Activity=Activity.Follow },
                new Acknowledgement() {Person="GlennStephens", Activity=Activity.WriteTheThemeTune },
                new Acknowledgement() {Person="Iceist", Activity=Activity.Follow },
                new Acknowledgement() {Person="ScottDev", Activity=Activity.DerailedEverything },
                new Acknowledgement() {Person="ScottDev", Activity=Activity.GiftSub },
                new Acknowledgement() {Person="ScottDev", Activity=Activity.Subscribe },
                new Acknowledgement() {Person="varrathien", Activity=Activity.Subscribe },
                new Acknowledgement() {Person="jhsebas", Activity=Activity.Follow },
                new Acknowledgement() {Person="varrathien", Activity=Activity.Follow },
                new Acknowledgement() {Person="DavidWengier", Activity=Activity.Raid},
                new Acknowledgement() {Person="BitwyzeGaming", Activity=Activity.Follow},
                new Acknowledgement() {Person="eldoen", Activity=Activity.Follow},


                // 26 August 2020
                new Acknowledgement() {Person="kenthtet", Activity=Activity.Follow},
                new Acknowledgement() {Person="zetawop", Activity=Activity.Follow},
                new Acknowledgement() {Person="DavidWengier", Activity=Activity.Raid},
                new Acknowledgement() {Person="scarlettcode", Activity=Activity.Follow},

                // 12 August 2020
                new Acknowledgement() {Person="agentesimoes", Activity=Activity.Follow},
                new Acknowledgement() {Person="xxxless", Activity=Activity.Follow },
                new Acknowledgement() {Person="Tondi", Activity=Activity.Follow },
                new Acknowledgement() {Person="nativelinux", Activity=Activity.Follow },
                new Acknowledgement() {Person="tbdgamer", Activity=Activity.DerailedEverything },
                new Acknowledgement() {Person="BenBtg", Activity=Activity.Follow},
                new Acknowledgement() {Person="Simontaga", Activity=Activity.Follow},
                new Acknowledgement() {Person="Simontaga", Activity=Activity.Follow},
                new Acknowledgement() {Person="le_BigSid", Activity=Activity.Follow},
                new Acknowledgement() {Person="EdNascimento31", Activity=Activity.Follow},
                new Acknowledgement() {Person="bricemarcelkouadio", Activity=Activity.Follow},
                new Acknowledgement() {Person="Asterisix", Activity=Activity.Subscribe},
                new Acknowledgement() {Person="zaploator", Activity=Activity.Follow},
                new Acknowledgement() {Person="marcosapoggi", Activity=Activity.Follow},
                new Acknowledgement() {Person="skullteria", Activity=Activity.Follow},
                new Acknowledgement() {Person="elianax_2000", Activity=Activity.Follow},
                new Acknowledgement() {Person="r2_au", Activity=Activity.Bitties},
                new Acknowledgement() {Person="jfversluis", Activity=Activity.Bitties},
                new Acknowledgement() {Person="jfversluis", Activity=Activity.Subscribe},
                new Acknowledgement() {Person="mdebruin93", Activity=Activity.Follow},
                new Acknowledgement() {Person="@vortexcdn", Activity=Activity.Follow},
                new Acknowledgement() {Person="@programmetajs", Activity=Activity.Follow},
                new Acknowledgement() {Person="@KevinQAnderson", Activity=Activity.Subscribe},


                // 5 August 2020
                new Acknowledgement() {Person="riallymundane", Activity=Activity.Raid},
                new Acknowledgement() {Person="Superbeer1994", Activity=Activity.Follow},
                new Acknowledgement() {Person="informationrocket", Activity=Activity.Follow},
                new Acknowledgement() {Person="STuaRTRuLes", Activity=Activity.Follow},
                new Acknowledgement() {Person="StevenThewissen", Activity=Activity.Follow},
                new Acknowledgement() {Person="fr33k3r", Activity=Activity.Follow},
                new Acknowledgement() {Person="DavidWengier", Activity=Activity.Raid},
                new Acknowledgement() {Person="deva73", Activity=Activity.Follow},
                new Acknowledgement() {Person="pickpie", Activity=Activity.Follow},
                new Acknowledgement() {Person="vishop_vishwa", Activity=Activity.Follow},
                new Acknowledgement() {Person="codingwithLuce", Activity=Activity.PullRequest},
            };
        }
    }
}
