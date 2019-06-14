<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PolicyView.aspx.cs" Inherits="WebApplication18.Views.Pages.PolicyView" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
   <div class="container">
            <div id="Policies" class ="col-md-12">  
            </div>
        <div id="addPolicy" class ="col-md-12">
            </div> 
       <div id="complex" class ="col-md-12">
            </div>  
        </div>
      <script type="text/javascript">

        $(document).ready(function () {
            
            var mainDiv = document.getElementById('Policies');
            var addPolicyDiv = document.getElementById('addPolicy');
            var complex = document.getElementById('complex');
               
               var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                var storeID =<%=ViewData["storeID"]%>;
              
                console.log("before jquery");
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/StorePolicies?storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (response) {
                            if (response != "") {
                                console.log(response);

                                var str = "<table class =\"table table-bordered text-center\">"
                                console.log("str:::::" + str + "\n");
                                str = str + "<tbody>";
                                var policies = response.split(";");
                                for (i = 0; i < policies.length - 1; i++) {
                                    var policyfields = policies[i].split(",");
                                    var type = policyfields[0];
                                    var description = policyfields[1];

                                    if (type == "Complex") {
                                        str +=
                                            "<tr>" +
                                            "<td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td></tr>";
                                    }
                                    
                                    else {
                                        str +=
                                            "<tr>" +
                                            "<td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td></tr>";
                                    }
                                }
               
                                str += " </tbody>" + "</table>";
                                mainDiv.innerHTML = str;
                                var str = "";
                               
                        }
                        else {
                                mainDiv.innerHTML = "<div align=\"center\"> <img src=\"../Images/Policy.png\"" + "height=\"400\" /></div>";
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          });

         

    </script>
                          
</asp:Content>