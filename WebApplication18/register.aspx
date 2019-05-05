<%@ Page Title="register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="WebApplication18.WebForm1" %>

<asp:Content ID="BodyContent1"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
        <asp:TextBox ID="TextBox1" name="TextBox1" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox2" name="TextBox2" runat="server"></asp:TextBox>
    </h2>
    <asp:Button ID="Button3" name="Button3" runat="server" Text="Button" OnClientClick="javascript:register()"/>
    <div >
      <input  type="password" name="name" id="name" placeholder="name">    
      </div>
    <div >
      <input  type="password" name="pass" id="pass" placeholder="123456">    
      </div>
    <div>
    <input type="button"  name="btnregister" id="btnregister" value="register" />
     </div>
  
     <script type="text/javascript">

        $(document).ready(function () {
            
            $("#btnregister").click(function () {
                 var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                username = $("#name").val();
                pass = $("#pass").val();

                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/user/register?username=" + username + "&password=" + pass,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        console.log(response); 
                        //console.log(response[1]);
                        if (response == "ok") {
                            //document.cookie = "HashCode=" + response[1]; //saves the hash code as a cookie
                              
                            window.location.href = baseUrl+"/";
                        }
                        else {
                            $("#registerAlert").html('Failure - ' + response);
                        }
                    },
                    error: function (response) {
                        console.log(response);
                        window.location.href = baseUrl+"/error";
                    }
                });
            });
        });

    </script>

</asp:Content>

