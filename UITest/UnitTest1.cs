using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
namespace UITest
{
    public class Page
    {
        public string Url;
        IWebDriver webDriver = new ChromeDriver(".");
     
        public void ChangeLang(string Lg)
        {
            webDriver.Url = Url;
            IWebElement OnButton = webDriver.FindElement(By.CssSelector("button[data-modal-id='language-selection']"));
            OnButton.Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            webDriver.FindElement(By.LinkText(Lg)).Click();
        }
        public void ChangeCurrency(string Cr)
        {
            webDriver.Url = Url;
            IWebElement OnButton = webDriver.FindElement(By.CssSelector("button[data-modal-aria-label='Выберите валюту']"));
            OnButton.Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            webDriver.FindElement(By.PartialLinkText(Cr)).Click();
        }
        public void Autorization(string mail)
        {
            webDriver.Url = Url;
         webDriver.FindElement(By.LinkText("Войти в аккаунт")).Click();
            webDriver.FindElement(By.CssSelector("input[name='username']")).SendKeys(mail);
            webDriver.FindElement(By.CssSelector("button[class='bui-button bui-button--large bui-button--wide']")).Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            webDriver.FindElement(By.CssSelector("button[class='bui-button nw-link-sign-in-without-pass bui-button--secondary bui-button--large bui-button--wide']")).Click();
        }



        public bool OnFlight()
        {
            webDriver.Url = Url;
            webDriver.FindElement(By.LinkText("Авиабилеты")).Click();                    
         if (webDriver.Url.Substring(0, 25)== "https://booking.kayak.com") { return true; } else { return false; }
        }


        public bool Filter(string City,string There,string Back,int Parents, int Childs, int Rooms)
        {
            string[] strs = There.Split('.');
            There = "";
            for (int i = strs.Length - 1; i >= 0; i--)
            {
                There = There + strs[i] + '-';
            }
            There = There.Substring(0, There.Length - 1);

            strs = Back.Split('.');
            Back = "";
            for (int i = strs.Length - 1; i >= 0; i--)
            {
                Back = Back + strs[i] + '-';
            }
            Back = Back.Substring(0, Back.Length - 1);
            webDriver.Url = Url;
            IWebElement OnCity = webDriver.FindElement(By.CssSelector("input[placeholder='Куда вы хотите поехать?']"));
            OnCity.SendKeys(City);
            webDriver.FindElement(By.CssSelector("span[class='sb-date-field__icon sb-date-field__icon-btn bk-svg-wrapper calendar-restructure-sb']")).Click();
            webDriver.FindElement(By.CssSelector("td[data-date='"+ There + "']")).Click();
            webDriver.FindElement(By.CssSelector("td[data-date='"+ Back + "']")).Click();
            webDriver.FindElement(By.CssSelector("label[class='xp__input']")).Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
           webDriver.FindElement(By.CssSelector("button[aria-label='Детей: увеличить количество']")).Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            webDriver.FindElement(By.ClassName("xp__button")).Click();
            webDriver.FindElement(By.ClassName("sb-date-field__display")).Click();
            IWebElement tdThere = webDriver.FindElement(By.XPath("//td[@data-date='"+ There+"']"));
           string checkedThere = tdThere.FindElement(By.XPath(".//span")).GetAttribute("aria-checked");
            IWebElement tdBack = webDriver.FindElement(By.XPath("//td[@data-date='" + Back + "']"));
            string checkedBack = tdBack.FindElement(By.XPath(".//span")).GetAttribute("aria-checked");
            IWebElement SelectP = webDriver.FindElement(By.XPath("//select[@name='group_adults']"));
            string SelectPtext = SelectP.FindElement(By.XPath(".//option[@value='"+Parents+"']")).Selected.ToString();
            IWebElement SelectC = webDriver.FindElement(By.XPath("//select[@name='group_children']"));
            string SelectCtext = SelectC.FindElement(By.XPath(".//option[@value='" + Childs + "']")).Selected.ToString();
            IWebElement SelectR = webDriver.FindElement(By.XPath("//select[@name='no_rooms']"));
            string SelectRtext = SelectR.FindElement(By.XPath(".//option[@value='" + Rooms + "']")).Selected.ToString();

            if ((checkedThere == "true") && (checkedBack == "true"))
            {
                if (webDriver.FindElement(By.CssSelector("input[class='c-autocomplete__input sb-searchbox__input sb-destination__input']")).GetAttribute("value") == City)
                {
                    if ((SelectPtext=="True")&& (SelectCtext == "True")&& (SelectRtext == "True"))
                    {
                        return true;
                    }
                }
              
            }
             return false;
        }
        public void Close()
        {
            webDriver.Close();
            webDriver.Quit();
        }

    }
    public class Tests
    {

        Page booking = new Page { Url = "https://www.booking.com/index.ru.html" };
        [Test]
        public void Test1()
        {
       
         booking.ChangeLang("Polski");
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
 booking.ChangeCurrency("EUR");
            Assert.Pass();
        }
        [Test]
        public void Test3()
        {
            if (booking.OnFlight() == true)
            {
                Assert.Pass();
            }
            else Assert.Fail();
        }
        [Test]
        public void Test4()
        {
            booking.Autorization("testmail@gmail.com");
            Assert.Pass();
        }
        [Test]
        public void Test5()
        {

            DateTime DtThere = DateTime.Now;
            DtThere= DtThere.AddDays(7);
            DateTime DtBack = DtThere.AddDays(2);
            string There = DtThere.ToShortDateString();
            string Back = DtBack.ToShortDateString();
         bool Filt=   booking.Filter("Минск", There, Back, 2,1,1);
            if (Filt == true)
            {
                Assert.Pass();
            }
            else Assert.Fail();


        }
        [OneTimeTearDown]
        public void CloseTest()
        {

   booking.Close();
        }

    }
}
