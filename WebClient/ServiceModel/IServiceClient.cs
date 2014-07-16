using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WebClient.ICS.Client.ServiceReference;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.ServiceModel
{
    /// <summary>
    /// IServiceClient.
    /// </summary>
    public interface IServiceClient
    {
        /// <summary>
        /// Enumerate child elements of tree settings.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="func"></param>
        void GetChildElements(Node parent, Action<IEnumerable<Element>, string> func);

        /// <summary>
        /// Create settings element.
        /// </summary>
        /// <param name="parent">Parent node.</param>
        /// <param name="newElement">New element.</param>
        /// <param name="parentViewModel">Parent view model.</param>
        /// <param name="func">Callback func.</param>
        void CreateElement(Node parent, Element newElement,
                           SettingNodeViewModel parentViewModel,
                           Action<Element, string, SettingNodeViewModel> func);

        /// <summary>
        /// Updates element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="func"></param>
        void GetElement(Element element, Action<Element, string> func);

        /// <summary>
        /// UpdateElement.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="func"></param>
        void UpdateElement(Element element, Action<Element, string> func);

        /// <summary>
        /// GetVersion.
        /// </summary>
        /// <param name="func"></param>
        void GetVersion(Action<string> func);

        /// <summary>
        /// GetUserPermissions.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="func"></param>
        void GetUserPermissions(User user, Action<IEnumerable<Permission>, string> func);

        /// <summary>
        /// GetElementPermissions.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="func"></param>
        void GetElementPermissions(Element element, Action<IEnumerable<Permission>, string> func);

        /// <summary>
        /// UpdatePermission.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="func">1: UserName, 2: NewPermission, 3: Message</param>
        void UpdatePermission(Permission permission, Action<string, Permission, string> func);
        
        /// <summary>
        /// AddPermission.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="permission"></param>
        /// <param name="func"></param>
        void AddPermission(Element element, Permission permission, Action<Permission, string> func);

        /// <summary>
        /// GetPermission
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="func"></param>
        void GetPermission(Permission permission, Action<Permission, string> func);

        /// <summary>
        /// DeletePermission.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="func"></param>
        void DeletePermission(Permission permission, Action<Permission, string> func);

        /// <summary>
        /// GetUsers.
        /// </summary>
        /// <param name="func"></param>
        void GetUsers(Action<IEnumerable<User>, string> func);

        /// <summary>
        /// DeleteElement.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="settingNode"></param>
        /// <param name="func"></param>
        void DeleteElement(Element element,
                           SettingNodeViewModel settingNode,
                           Action<SettingNodeViewModel, string> func);

        /// <summary>
        /// DeleteUser.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="func"></param>
        void DeleteUser(User user, Action<string> func);

        /// <summary>
        /// GenerateScript.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="stream"></param>
        /// <param name="func"></param>
        void GenerateScript(Node element, TextWriter stream, Action<string, TextWriter, string> func);

        /// <summary>
        /// ApplyScript.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="script"></param>
        /// <param name="func"></param>
        void ApplyScript(Node element, string script, Action<string> func);

        /// <summary>
        /// GetElementHistory.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="successfull"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="userFilter"></param>
        /// <param name="operationFilter"></param>
        /// <param name="max"></param>
        /// <param name="func"></param>
        void GetElementHistory(Element element, string successfull, DateTime from, DateTime to,
                                               string userFilter, string operationFilter, int max,
                                               Action<ObservableCollection<HistoryInfo>, string> func);

        /// <summary>
        /// GetHistoryRecords.
        /// </summary>
        /// <param name="historyInfo"></param>
        /// <param name="func"></param>
        void GetHistoryRecords(HistoryInfo historyInfo, Action<ObservableCollection<HistoryRecord>, string> func);

        /// <summary>
        /// GetElementSubscriptions.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="func"></param>
        void GetElementSubscriptions(Element  element, Action<ObservableCollection<Subscription>, string> func);

        /// <summary>
        /// UpdateSubscription.
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="func"></param>
        void UpdateSubscription(Subscription subscription, Action<Subscription, string> func);

        /// <summary>
        /// DeleteSubscription.
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="func"></param>
        void DeleteSubscription(Subscription subscription, Action<string> func);

        /// <summary>
        /// CreateSubscription.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="subscription"></param>
        /// <param name="func"></param>
        void CreateSubscription(Element element, Subscription subscription, Action<Subscription, string> func);

        /// <summary>
        /// GetSubscription.
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="func"></param>
        void GetSubscription(Subscription subscription, Action<Subscription, string> func);

        /// <summary>
        /// GetUserSubscriptions.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="func"></param>
        void GetUserSubscriptions(User user, Action<ObservableCollection<Subscription>, string> func);
    }
}