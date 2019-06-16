<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StorePolicy.aspx.cs" Inherits="WebApplication18.Views.Pages.StorePolicy" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
    <style>
        .quantity {
            float: left;
            margin-right: 15px;
            background-color: #eee;
            position: relative;
            width: 80px;
            overflow: hidden
        }

    .quantity input {
            margin: 0;
            text-align: center;
            width: 20px;
            height: 20px;
            padding: 0;
            float: right;
            color: #000;
            font-size: 20px;
            border: 0;
            outline: 0;
            background-color: #F6F6F6
        }

        .quantity input.qty {
            position: relative;
            border: 0;
            width: 100%;
            height: 40px;
            padding: 10px 25px 10px 10px;
            text-align: center;
            font-weight: 400;
            font-size: 15px;
            border-radius: 0;
            background-clip: padding-box
        }

    .quantity.minus, .quantity.plus {
            line-height: 0;
            background-clip: padding-box;
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0;

            color: #bbb;
            font-size: 20px;
            position: absolute;
            height: 50%;
            border: 0;
            right: 0;
            padding: 0;
            width: 25px;
            z-index: 3
        }

        .quantity.minus :hover, .quantity.plus :hover {
            background-color: #dad8da
        }

    .quantity.minus {
            bottom: 0
        }

.shopping-cart {
            margin-top: 20px;
        }
.social-part.fa {
            padding-right: 20px;
        }
</style>
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
                                    var amount = policyfields[2];
                                    var policyID = policyfields[3];
                                    if (type == "Complex Policy") {
                                        str +=
                                            "<tr>" +
                                            "<td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td><td style = \"width:50px\"><div></div></td><td style = \"width:50px\" align=\"center\"> <button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow(" + policyID + ");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></td></tr>";
                                    }
                                    
                                    else {
                                        str +=
                                            "<tr>" +
                                            "<td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td><td style = \"width:50px\"> <div class=\"quantity\"><input type=\"button\" value=\"+\" onclick=\"plusAmount(" + policyID + "," + amount + ");\" class=\"plus\"><input type=\"text\" value=\"" + amount + "\" title=\"Qty\" class=\"qty\"><input type=\"button\" value=\"-\" onclick=\"minusAmount(" + policyID + "," + amount + ");\" class=\"minus\"></div></td><td style = \"width:50px\" align=\"center\"> <button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow(" + policyID + ");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></td></tr>";
                                    }
                                }
               
                                str += " </tbody>" + "</table>";
                                mainDiv.innerHTML = str;
                                var str = "";
                                addPolicyDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" ><input type=\"button\" class=\"btn btn-secondary\" value=\"Add Policy\" onclick=\" addPolicy("+storeID+")\"></div>";
                                complex.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" ><input type=\"button\" class=\"btn btn-secondary\" value=\"Complex Policy\" onclick=\" complex()\"></div>";

                        }
                        else {
                                mainDiv.innerHTML = "<div align=\"center\"> <img src=\"../WebApplication18/Images/Policy.png\"" + "height=\"400\" /></div>";
                                addPolicyDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" > <h5><strong>Store does not have Policies</strong></h5> <input type=\"button\" class=\"btn btn-secondary\" value=\"Add Policy\" onclick=\" addPolicy("+storeID+")\"></div>";

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
              var storeID =<%=ViewData["storeID"]%>;
              var plusAmount = amount + 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/setPolicyAmount?policyID=" + id + "&amount=" + plusAmount + "&storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            alert(response);
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        alert(response);

                        console.log(response);
                    }
                });
          }
          
          function minusAmount(id, amount) {
              event.preventDefault();
              console.log(id);
              var storeID =<%=ViewData["storeID"]%>;
              var minusAmount = amount - 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/setPolicyAmount?policyID=" + id + "&amount=" + minusAmount + "&storeID=" + storeID,
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
                    url: baseUrl+"/api/store/removePolicy?policyID=" + id +"&storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {
                            alert("Policy removed seccessfully");

                            location.reload();
                        }
                        else {
                            alert(response);

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
               window.location.href = baseUrl+"/ComplexPolicy?storeId=" + storeID;
          }

          
       function addPolicy(storeID) {         
               event.preventDefault();
               var getUrl = window.location;
           var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
           window.location.href = baseUrl + "/EditStorePolicy?storeId=" + storeID;
       }

    </script>
                          
</asp:Content>