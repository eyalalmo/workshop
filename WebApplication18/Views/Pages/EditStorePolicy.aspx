<%@ Page  Title="Edit Store Policy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditStorePolicy.aspx.cs" Inherits="WebApplication18.Views.Pages.EditStorePolicy" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
     <h2><%: Title +" id:  " + ViewData["storeId"] %>
    </h2>
    <div class="form-group">
  <label for="usr">Max Quantity:</label>
  <input type="number" class="form-control" id="max" name="name">
</div>
 <button type="submit" name="btnAdd" class="btn btn-primary"  id="maxBtn" >Set</button>
  <div class="form-group">
      <br />
   <label for="usr">Min Quantity:</label>
  <input type="number" class="form-control"  id="min" name="name">
  </div>
  <button type="submit" name="btnAdd" class="btn btn-primary" id="minBtn" >Set</button>
 
   <script type="text/javascript">

        $(document).ready(function () {
            
            $("#minBtn").click(function () {
               event.preventDefault();
             var storeID  =<%=ViewData["storeId"]%>;
              var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
             
                maxVal = $("#max").val();

                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/SetMaxPolicy?storeID=" + storeID + "&maxVal=" + maxVal,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response === "ok") {
                            alert("store policy changed success")
                        }
                        else {
                            alert("store policy changed failes")    
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });


            $("#maxBtn").click(function () {
             event.preventDefault();
             var storeID  =<%=ViewData["storeId"]%>;
             var getUrl = window.location;
             var baseUrl = getUrl.protocol + "//" + getUrl.host
              
              minVal = $("#min").val();

                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/SetMinPolicy?storeID=" + storeID + "&minVal=" + minVal,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response === "ok") {
                            alert("store policy changed success")
                        }
                        else {
                            alert("store policy changed failes")    
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        });

    </script>

</asp:Content>


