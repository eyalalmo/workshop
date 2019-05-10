<%@ Page Language="C#" Title="Admin Panel" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AdminPan.aspx.cs" Inherits="WebApplication18.Views.Pages.AdminPan" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
        
    </h2>
    
  <div class="form-group">
    <label>Enter user name you wish to remove:</label>
    <input class="form-control" id="username" name="pass">
  </div>
        <button id="userDelete" type="button" class="btn btn-danger">Remove</button>
   <script type="text/javascript">

        $(document).ready(function () {
            $("#userDelete").click(function () {
                event.preventDefault();
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                username = $("#username").val();

                if (confirm("Are you sure you want to remove " + username + " ?")) {
                    jQuery.ajax({
                        type: "GET",
                        url: baseUrl + "/api/user/removeUser?username=" + username,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {

                            if (response == "ok") {
                                window.location.href = baseUrl + "/";
                            }
                            else {
                                alert(response);
                            }
                        },
                        error: function (response) {
                            console.log(response);
                        }
                    });
                }
            });
        });

    </script>

    

   <script type="text/javascript">


       $(document).ready(function () {
           $("#del1").click(function () {
                event.preventDefault();
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                if (confirm("Are you sure you want to delete ____ ?")) {
                    jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/AdminPan/removeUser?username=nfds",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response == "ok?") {   
                            window.location.href = baseUrl+"/api/Default/";
                        }
                        else {
                            alert(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
                } else {
                    ////////
                }
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
            });
        });

    </script>

</asp:Content>
