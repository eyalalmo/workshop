<%@ Page Title="Store Staff Panel" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStaff.aspx.cs" Inherits="WebApplication18.Views.Pages.ManageStaff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3><%: Title %></h3>   
    Store id: <font style="color:red"><b><%: ViewData["storeId"] %></b></font>

    <table>
        <tr>
            <td style="vertical-align:top; width:300px" id="owners"></td>
            <td style="vertical-align:top; width:300px" id="managers"></td>
            <td style="vertical-align:top" id="pending"></td>
        </tr>
    </table>
    
    <script type="text/javascript">
 
        function getRoles(response) {
            var doc = document.getElementById('allRoles');
            var i;
            var jsonList = JSON.parse(response);
            var OWNERS = "<h3><u>Owners: </u></h3>";
            var MANAGERS= "<h3><u>Managers: </u></h3>";
            for (i = 0; i < jsonList.length; i++) {

                if (jsonList[i].isOwner) {
                    OWNERS += `<tr>
                                        <font style="font-size:16px"><b>` + jsonList[i].userName.username + `</b></font>
		                       <tr></br>`;
                }
                else
                {
                    MANAGERS += `<tr>
                                    <font style="font-size:16px"><b>` + jsonList[i].userName.username + `</b></font> (Appointed by ` + jsonList[i].appointedBy.username + `)
                                    <button type="button" class="btn btn-danger" onClick="removeRole(\'` + jsonList[i].userName.username + `\' )"> Remove</button>
                                 </tr></br>`;
                }
            }
            OWNERS += `<div clas="row"><input type="text" size="12" visible="false" placeholder="Username"  id="owner" name="owner"/>
                       <button type="button" class="btn btn-primary" onClick="confirmOwner()" id="confirm" name="confirm"/>Add owner</div>`
            MANAGERS += `<button type="button"  name="addManager" id="addManager" class="btn btn-primary" data-toggle="modal" data-target="#myModal">Add Manager</button>`
            document.getElementById('owners').innerHTML = OWNERS;
            document.getElementById('managers').innerHTML = MANAGERS;           
            
        };

        function getPending(response) {
            var doc = document.getElementById('allPending');
            var i;
            console.log(response);
            var jsonList = JSON.parse(response);
            var HTML = "";
            
            if (jsonList.length > 0) {
                HTML+="<h3><u>Owners: </u></h3>";
            }
            for (i = 0; i < jsonList.length; i++) {
                HTML += `<tr>
		                    <td>`+ jsonList[i] + `</td>
                            <td><button type="button" class="btn btn-success" onClick="signContract( \'`+ jsonList[i] +`\' )"/>
                             Sign Contract
                            </button></td>
                            <td><button type="button" class="btn btn-danger" onClick="declineContract( \'`+ jsonList[i] +`\' )"/>
                             declineContract
                            </button></td>
                        </tr>`
        
            }
            document.getElementById('pending').innerHTML += HTML;

            
        };
        function addM() {
                    var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;
            var username = $("#manager").val();
            var prod = $("#per1").is(':checked'); 
            var disc = $("#per2").is(':checked'); 
            var poli = $("#per3").is(':checked'); 

        
            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/addManager?username=" + username + "&storeID=" + storeId + "&prod=" + prod + "&disc=" + disc + "&poli=" + poli,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Manager successfuly  added")
                     window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        };
    </script>
    <script type="text/javascript">
        function removeRole(username) {

            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/removeRole?username=" + username + "&storeID=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Staff member successfuly  removed")
                        window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function signContract(username) {

            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/signContract?username=" + username + "&storeID=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Contract successfuly signed")
                        window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        }

        function declineContract(username) {

            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var storeId =<%=ViewData["storeId"]%>;

            jQuery.ajax({
                type: "GET",
                url: baseUrl + "/api/store/declineContract?username=" + username + "&storeID=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response !== "ok") {
                        alert(response);
                    } else
                        alert("Declined the contract. The user will not be appointed as an owner")
                        window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;

                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
        function confirmOwner() {
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var username = $("#owner").val();
            var storeId =<%=ViewData["storeId"]%>;
                
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/addOwner?username=" + username +"&storeID=" + storeId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response !== "ok") {
                            alert(response);
                        } else
                            alert("User has been successfuly offered as a store owner. Contract requests sent to all the other relevant owners")
                            window.location.href = baseUrl+"/ManageStaff?storeId="+storeId ;
                        
                    },
                    error: function (response) {
                        console.log(response);
                    }
            });
            
            };

    $(document).ready(function () {

        var storeId =<%=ViewData["storeId"]%>;
        var getUrl = window.location;
        var baseUrl = getUrl.protocol + "//" + getUrl.host
        jQuery.ajax({
            type: "GET",
            url: baseUrl + "/api/store/getAllRoles?storeId=" + storeId,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                getRoles(response);

            },
            error: function (response) {
                console.log(response);
            }
        });
        jQuery.ajax({
            type: "GET",
            url: baseUrl + "/api/store/getAllPending?storeId=" + storeId,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                getPending(response);

            },
            error: function (response) {
                console.log(response);
            }
        });




    });

    </script>
    </asp:Content>