<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoreDiscounts.aspx.cs" Inherits="WebApplication18.Views.Pages.StoreDiscounts" %>


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
                            console.log("111111111111111111\n");
                            if (response != "") {
                                console.log(response);

                                var str = "<table class =\"table table-bordered text-center\">"
                                console.log("str:::::" + str + "\n");
                                str = str + "<tbody>";
                                var discounts = response.split(";");
                                for (i = 0; i < discounts.length - 1; i++) {
                                    var discountfields = discounts[i].split(",");
                                    var type = discountfields[0];
                                    var description = discountfields[1];
                                    var percentage = discountfields[2];
                                    var duration = discountfields[3];
                                    var discountID = discountfields[4];
                                    if (type == "Complex") {
                                        str +=
                                            "<tr>" +
                                            "<td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td><td style = \"width:120px\;top:100px; align=\"center\">" + duration + "</td><td style = \"width:50px\"><div></div></td><td style = \"width:50px\" align=\"center\"> <button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow(" + discountID + ");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></td></tr>";
                                    }
                                    
                                    else {
                                        str +=
                                            "<tr>" +
                                            "<td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td><td style = \"width:120px\;top:100px; align=\"center\">" + duration + "</td><td style = \"width:50px\"> <div class=\"quantity\"><input type=\"button\" value=\"+\" onclick=\"plusQuantity(" + discountID + "," + percentage + ");\" class=\"plus\"><input type=\"text\" value=\"" + percentage + " %\" title=\"Qty\" class=\"qty\"><input type=\"button\" value=\"-\" onclick=\"minusQuantity(" + discountID + "," + percentage + ");\" class=\"minus\"></div></td><td style = \"width:50px\" align=\"center\"> <button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow(" + discountID + ");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></td></tr>";
                                    }
                                }
               
                                str += " </tbody>" + "</table>";
                                mainDiv.innerHTML = str;
                                var str = "";
                                addDiscountDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" ><input type=\"button\" class=\"btn btn-secondary\" value=\"Add Discount\" onclick=\" addDiscount("+storeID+")\"></div>";
                                complex.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" ><input type=\"button\" class=\"btn btn-secondary\" value=\"Complex Discount\" onclick=\" complex()\"></div>";

                        }
                        else {
                                mainDiv.innerHTML = "<div align=\"center\"> <img src=\"../Images/nodiscount_2.png\"" + "height=\"400\" /></div>";
                                addDiscountDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" > <h5><strong>Store does not have Discounts</strong></h5> <input type=\"button\" class=\"btn btn-secondary\" value=\"Add Discount\" onclick=\" addDiscount("+storeID+")\"></div>";

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          });

          //<td><div class=\"col-4 col-sm-4 col-md-4 text-right\"><input type=\"button\"  class=\"btn btn-secondary\" value=\"Complex\" onclick=\"complex();\"></div></td>

          function plusQuantity(id, percentage) {
              event.preventDefault();
              console.log(id);
              var plusPercentage = percentage + 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/setDiscountPercentage?discountID=" + id+"&percentage="+plusPercentage,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }
          
          function minusQuantity(id, percentage) {
              event.preventDefault();
              console.log(id);
              var minusPercentage = percentage - 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/setDiscountPercentage?discountID=" + id+"&percentage="+minusPercentage,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }

          function deleteRow(id) {
              console.log("!!!!" + id);
              event.preventDefault();
              console.log("!!!!" + id);
                var getUrl = window.location;
                var storeID =<%=ViewData["storeID"]%>;
                console.log("1");
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                   type: "GET",
                    url: baseUrl+"/api/store/removeDiscount?discountID=" + id +"&storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }
           function complex() {
               event.preventDefault();
               var storeID =<%=ViewData["storeID"]%>;
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               window.location.href = baseUrl+"/ComplexDiscount?storeId=" + storeID;
          }

          
          function complex2() {
              event.preventDefault();
                var getUrl = window.location;
                var storeID =<%=ViewData["storeID"]%>;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                   type: "GET",
                    url: baseUrl+"/api/store/complexDiscount?discountID1=1&discountID2=2"+"&storeID="+storeID+"&type=and",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }

          
       function addDiscount(storeID) {         
               event.preventDefault();
               var getUrl = window.location;
           var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
           window.location.href = baseUrl + "/StoreDiscount?storeId=" + storeID;
       }

    </script>
                          
</asp:Content>