<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComplexPolicy.aspx.cs" Inherits="WebApplication18.Views.Pages.ComplexPolicy" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
   <div class="container">
            <div id="Discounts" class ="col-md-12">  
            </div>
          <div id="t" class="pull-right" style = "margin: 10px"><input type="text" class="form-control" id="type" name="type"></div>
         <div class="pull-right" style = "margin: 10px"><h5><strong>Type</strong> <small>(and/or/xor)</small></h5></div>
       <div id="d" class="pull-right" style = "margin: 10px"></div>
        <div id="makeComplex" class ="col-md-12">
            </div>   
        </div>
      <script type="text/javascript">

        $(document).ready(function () {
            
            var mainDiv = document.getElementById('Discounts');
            var makeComplexDiv = document.getElementById('makeComplex');

               
               var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
                var storeID =<%=ViewData["storeID"]%>;
              
                console.log("before jquery");
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/StorePolicies?storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (response) {
                            console.log("response");
                            if (response != "") {
                                console.log(response);

                                var str ="<table class =\"table table-bordered text-center\"><tbody>";
                                var policies = response.split(";");
                                for (i = 0; i < policies.length - 1; i++) {
                                    var policyfields = policies[i].split(",");
                                    var type = policyfields[0];
                                    var description = policyfields[1];
                                    var amount = policyfields[2];
                                    var policyID = policyfields[3];
                                    if (type == "Complex") {
                                        str += "<tr>" +
                                            "<td  style = \"width:30px\"><input type=\"checkbox\"></td><td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td><td style = \"width:50px\"><div></div></td><td style = \"width:50px\" align=\"center\"> <button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow(" + policyID + ");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></td></tr>";
                                    }
                                    else {
                                        str += "<tr>" +
                                            "<td  style = \"width:30px\"><input type=\"checkbox\"></td><td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td><td style = \"width:50px\"> <div class=\"quantity\"><input type=\"button\" value=\"+\" onclick=\"plusAmount(" + policyID + "," + amount + ");\" class=\"plus\"><input type=\"text\" value=\"" + amount + "\" title=\"Qty\" class=\"qty\"><input type=\"button\" value=\"-\" onclick=\"minusAmount(" + policyID + "," + amount + ");\" class=\"minus\"></div></td><td style = \"width:50px\" align=\"center\"> <button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow(" + policyID + ");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></td></tr>";
                                    }
                                }
                                str += " </tbody>" + "</table>";
                                console.log(str);
                                mainDiv.innerHTML = str;
                                makeComplexDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" ><input type=\"button\" class=\"btn btn-secondary\" value=\"Make Complex\" onclick=\" makeComplex(" + storeID + ")\"></div>";

                        }
                        else {
                                mainDiv.innerHTML = "<div align=\"center\"> <img src=\"../WebApplication18/Images/nodiscount_2.png\"" + "height=\"400\" /></div>";
                                addDiscountDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" > <h5><strong>Store does not have Discounts</strong></h5> <input type=\"button\" class=\"btn btn-secondary\" value=\"Add Discount\" onclick=\" addDiscount("+storeID+")\"></div>";

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          });

          //<td><div class=\"col-4 col-sm-4 col-md-4 text-right\"><input type=\"button\"  class=\"btn btn-secondary\" value=\"Complex\" onclick=\"complex();\"></div></td>

          function plusAmount(id, amount) {
              event.preventDefault();
              console.log(id);
              var plusAmount = amount + 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/setPolicyAmount?policyID=" + id+"&amount="+plusAmount,
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
          
          function minusAmount(id, amount) {
              event.preventDefault();
              console.log(id);
              var minusAmount = amount - 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/setPolicyAmount?policyID=" + id+"&amount="+minusAmount,
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
              event.preventDefault();
                var getUrl = window.location;
                var storeID =<%=ViewData["storeID"]%>;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                   type: "GET",
                    url: baseUrl+"/api/store/removePolicy?policyID=" + id +"&storeID=" + storeID,
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
          
          function makeComplex() {
              event.preventDefault();
              var getUrl = window.location;
              var storeID =<%=ViewData["storeID"]%>;
              var s = "";
              type = $("#type").val();

              $('table [type="checkbox"]').each(function(i, chk) {
                    if (chk.checked) {
                        console.log("Checked!", i, chk);
                        s = s + i + " ";
                    }
                  });
              console.log("type::::" + type);
             console.log("Make COMPLEX!!!!");
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                   type: "GET",
                   url: baseUrl + "/api/store/complexPolicy?policies=" + s + "&storeID=" + storeID + "&type=" + type ,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {
                            event.preventDefault();
                            var getUrl = window.location;
                            var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
                            window.location.href = baseUrl+"/StorePolicy?storeID=" + storeID;
                        }
                        else {
                            console.log("ERROR:" + response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }
           
         

    </script>
                          
</asp:Content>