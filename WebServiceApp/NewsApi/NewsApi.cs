/*
 * Andrew Dorre
 * Final Programming Project
 * 
 * This program accesses a web service api to present up to date news
 * headlines, along with links, dates and the source that is reporting it
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
// There are apparently several ways to access and parse a JSON object
// I found using this NewtonSoft package was relatively simple. I installed it
// with NuGet as mentioned in the link below, and then I was able to get it to 
// do what I wanted pretty easily after checking the examples.
// https://stackoverflow.com/questions/6620165/how-can-i-parse-json-with-c
using Newtonsoft.Json.Linq;

namespace NewsApi
{
    public partial class NewsApi : Form
    {
        // This is a necessary object that creates a connection
        // to newsapi.org
        private HttpClient client = new HttpClient();

        // An object made to hold the current headlines, with the properties
        // that I want to show. those properties are the headline, the date published,
        // the source of the news, and the URL link. I made my own class to hold these
        // properties.
        List<NewsHeader> newsList = new List<NewsHeader>();

        public NewsApi()
        {
            InitializeComponent();
        }

        // Using async here, and await below, because the program will then "await"
        // the results returned from newsapi.org, and still run asynchronously without
        // locking up.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {            
            // The "dynamic stuff" is torn straight from the NewtonSoft examples, although
            // I changed other stuff, such as jsonNews.
            string jsonNews = await client.GetStringAsync(new Uri(
                "https://newsapi.org/v2/top-headlines?country=us&apiKey=5cb35fea7dfd4098abd2498570bdcfb9"));
            dynamic stuff = JObject.Parse(jsonNews);
            
            // This might have been overkill. I have multiple lists that hold all of the labels
            // on the form. I felt like this was necessary to keep all the individual components
            // of the stories lined up correctly. I was worried the title for one story might end up
            // with the link of another, etc.
            // With this method, I can run them all in the same loops parallel and avoid that issue,
            // although I do feel like there is a more efficient way
            List<Label> labelURL = new List<Label>();
            List<Label> labelTitle = new List<Label>();
            List<Label> labelDate = new List<Label>();
            List<Label> labelSource = new List<Label>();

            // This list holds ALL of the labels within the group box. I tried several methods of simply
            // getting the label objects by name (.Find(), I tried creating strings like "labelURL + i" where
            // i was an iterating integer to try and loop through my collection of labels, but nothing worked
            // This was probably my biggest headache. Finally I figured I could create a list to hold everything
            // and then filter out the different label types from that list, and remove them as I did.
            // Worked like a charm.
            List<dynamic> allControls = new List<dynamic>();

            // So here is a foreach to add all of the label controls to the "allControls" list
            // I researched various techniques to get to this point. I'm going to show the links,
            // although several of my original strategies ended up being deadends.
            // https://stackoverflow.com/questions/1536739/get-a-windows-forms-control-by-name-in-c-sharp 
            foreach (Label lbl in panel1.Controls)
            {
                if (lbl is Label)
                {
                    allControls.Add(lbl);
                }            }
            
            while (allControls.Count > 1)
            {
                for (int n = 1; n < 11; n++)
                {
                    // This one ended up helping me build the filter of the name for each label that I ultimately used,
                    // Although I was originally trying to use it for other reasons. All of this helped me get a lot better
                    // with foreach loops, which was something I used so little, I wasn't very confident with them. I definitely
                    // learned a lot about them now though.
                    // https://stackoverflow.com/questions/37480105/how-can-i-add-several-labels-to-a-list-in-a-for-loop

                    // Also, this was a gotcha. I almost thought this wouldn't work until I stumbled across this gem.
                    // I was getting the strange error mentioned here. I only sort of understand why this was an issue.
                    // Needed to add "ToList()" to the end of this apparently. I was just lucky it worked
                    // https://stackoverflow.com/questions/604831/collection-was-modified-enumeration-operation-may-not-execute
                    foreach (Label lbl in allControls.ToList())
                    {
                        if (lbl.Name.StartsWith("linkLabel") && lbl.Name.EndsWith(n.ToString()))
                        {
                            labelURL.Add(lbl);
                            allControls.Remove(lbl);
                        }
                        if (lbl.Name.StartsWith("lblDate") && lbl.Name.EndsWith(n.ToString()))
                        {
                            labelDate.Add(lbl);
                            allControls.Remove(lbl);
                        }
                        if (lbl.Name.StartsWith("lblSource") && lbl.Name.EndsWith(n.ToString()))
                        {
                            labelSource.Add(lbl);
                            allControls.Remove(lbl);
                        }
                        if (lbl.Name.StartsWith("lblTitle") && lbl.Name.EndsWith(n.ToString()))
                        {
                            labelTitle.Add(lbl);
                            allControls.Remove(lbl);
                        }
                    }
                }                
            }            
  
            // Here I'm adding the information directly from the json object to my NewsHeader object.
            // It seems like my class might be unnecessary. I could theoretically add the json
            // array info straight to the labels below probably, but this way I'm storing that info
            // in a secondary spot, and I feel like it might be useful.
            // Was trying to find the number of articles using .Length at first, but that doesn't work
            // Used .Count instead: https://stackoverflow.com/questions/19025174/get-length-of-array-json-net
            for (int i = 0; i < (stuff.articles).Count; i++)
            {
                NewsHeader testHeader = new NewsHeader();
                testHeader.NewsTitle = stuff.articles[i].title;
                testHeader.NewsDate = stuff.articles[i].publishedAt;
                testHeader.NewsSource = stuff.articles[i].source.name;
                testHeader.NewsLink = stuff.articles[i].url;
                newsList.Add(testHeader);
            }
            // This is just turning off the visibility of the welcome message, so it doesn't interfere
            // with the news feed
            lblInfo.Visible = false;

            // And here the labels are all populated with the news feed info. Then they are made visible
            for (int i = 0; i < 10; i++)
            {
                labelTitle[i].Text = newsList[i].NewsTitle;
                labelURL[i].Text = newsList[i].NewsLink;
                labelDate[i].Text = newsList[i].NewsDate;
                labelSource[i].Text = newsList[i].NewsSource;

                labelTitle[i].Visible = true;
                labelURL[i].Visible = true;
                labelDate[i].Visible = true;
                labelSource[i].Visible = true;
            }
        }

        // This one gave me a bit of trouble. I was really hoping I could just add a value with the
        // link to each linklabel. I find this a little weird that it isn't possible. After going
        // through a JavaScript class, this felt a lot more complicated to wrap my head around.
        // I think part of the reason though, is that this is a windows form that consumes a web service,
        // rather than a full web application.
        // I used this site to figure things out, although I definitely modified it for my own needs.
        // https://stackoverflow.com/questions/7039768/difference-between-linklabel-click-and-linklabel-linkclicked-event
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel2.Text);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel3.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel3.Text);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel4.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel4.Text);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel5.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel5.Text);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel6.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel6.Text);
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel7.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel7.Text);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel8.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel8.Text);
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel9.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel9.Text);
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel10.LinkVisited = true;
            System.Diagnostics.Process.Start(linkLabel10.Text);
        }
    }
}
