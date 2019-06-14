<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DiscountView.aspx.cs" Inherits="WebApplication18.Views.Pages.DiscountView" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
   <div class="container">
            <div id="Discounts" class ="col-md-12">  
            </div>
        <div id="addDiscount" class ="col-md-12">
            </div> 
       <div id="complex" class ="col-md-12">
            </div>  
        </div>
      <script type="text/javascript">

        $(document).ready(function () {
            
            var mainDiv = document.getElementById('Discounts');
            var addDiscountDiv = document.getElementById('addDiscount');
            var complex = document.getElementById('complex');
               
               var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
                var storeID =<%=ViewData["storeID"]%>;
              
                console.log("before jquery");
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/StoreDiscounts?storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (response) {
                            if (response != "") {
                                console.log(response);

                                var str = "<table class =\"table table-bordered text-center\">"
                                str = str + "<tbody>";
                                var discounts = response.split(";");
                                for (i = 0; i < discounts.length - 1; i++) {
                                    var discountfields = discounts[i].split(",");
                                    var type = discountfields[0];
                                    var description = discountfields[1];
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
                                mainDiv.innerHTML = "<div align=\"center\"> <img src=\"../Images/nodiscount_2.png\"" + "height=\"400\" /></div>";

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          });

          //<td><div class=\"col-4 col-sm-4 col-md-4 text-right\"><input type=\"button\"  class=\"btn btn-secondary\" value=\"Complex\" onclick=\"complex();\"></div></td>


    </script>
                          
</asp:Content>