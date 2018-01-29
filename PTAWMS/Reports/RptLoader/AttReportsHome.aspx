<%@ Page Title="" Language="C#" MasterPageFile="~/ReportingEngine.Master" AutoEventWireup="true" CodeBehind="AttReportsHome.aspx.cs" Inherits="PTAWMS.Reports.RptLoader.AttReportsHome" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .hiddencol
        {
        display: none;
        }
    </style>
    <div class="page-content">
        <div class="page-header">
            <h1>
                 <% if (HttpContext.Current.Session["ReportMenu"].ToString()=="Attendance")
                                 { %> Attendance Reports <%}
                    else if (HttpContext.Current.Session["ReportMenu"].ToString() == "Visitor")
                 { %> Visitor Reports <%}
                    else if (HttpContext.Current.Session["ReportMenu"].ToString() == "Overtime")
                 { %> Overtime Reports <%}%>
                <small>
                <i class="ace-icon fa fa-angle-double-right"></i>
                Detailed View
                </small>
            </h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-md-12">
                <div class="col-md-8">
                   <%-- <div class="row">
                        <div class="col-md-6">
                            <span class="h5">From :</span>  <input id="dateFrom"  class="input-sm"  runat="server" type="date" />
                        </div>
                        <div class="col-md-6">
                            <span class="h5">To :</span>  <input id="dateTo" class="input-sm"  runat="server" type="date" />
                        </div>
                        <hr />
                    </div>--%>
                    <%--<% if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
                             {%>
                    <div class="checkbox">
													<label>
                                                        <asp:CheckBox ID="chkDate" runat="server"/>
														<span class="lbl"> Apply Date Filters</span>
                                                        
													</label>
												</div>
                    <% }%>  --%>
                    <br />
                    <div class="row col-md-12">
                       <% { %> <% PTAWMS.Models.ViewUserEmp user = (PTAWMS.Models.ViewUserEmp)HttpContext.Current.Session["LoggedUser"];%>
<%--                        <% if(HttpContext.Current.Session["IsSupervisor"].ToString()=="0" ) %> 
                        <% {%>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-success" title="Generate Reports" OnClick="btnStepSix_Click" >Generate Report</asp:LinkButton>
                         <% } else{%>--%>
                         <% if (user.HRModule == true)
                             {%>
                        <asp:LinkButton ID="LinkButton2"  runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepOne_Click" ToolTip="Stations, Divisions">Location</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton6" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepTwo_Click" ToolTip="Types, Domicile, Shifts">Category</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton7" runat="server"  CssClass="btn  btn-sm btn-grey" OnClick="btnStepThree_Click" ToolTip="Department, Designations">Posting</asp:LinkButton>
                        

                        <% if (Convert.ToString(Session["ReportMenu"]) == "HRMS")
                            {%>
                        <asp:LinkButton ID="LinkButton8" runat="server" CssClass="btn  btn-sm btn-grey" OnClick="btnHRQStep_Click" ToolTip="Degree, Institute">Education</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton10" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepFour_Click" ToolTip="Employees">Employees</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton11" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnGenerateHRReport" >Generate Report</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton12" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnStepSix_Click" >Reports</asp:LinkButton>
                        <%}
                        else
    { %>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepFour_Click" ToolTip="Employees">Employees</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton9" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnStepSix_Click" >Reports</asp:LinkButton>
                        <%}%>
                        <%} else if(user.UserType=="A" || user.UserType=="H" ||user.UserType=="E") %> 
                        <% {%>        
                         <asp:LinkButton ID="btnStepOne"  runat="server" CssClass="btn btn-sm btn-grey" OnClick="btnStepOne_Click" >Stations, Divisions</asp:LinkButton>
                        <asp:LinkButton ID="btnStepTwo" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepTwo_Click" >Types, Shifts</asp:LinkButton>
                        <asp:LinkButton ID="btnStepThree" runat="server"  CssClass="btn  btn-sm btn-grey" OnClick="btnStepThree_Click" >Department, Designations</asp:LinkButton>
                        <asp:LinkButton ID="btnStepFour" runat="server" CssClass="btn btn-sm  btn-grey" OnClick="btnStepFour_Click" >Employees</asp:LinkButton>
                        <asp:LinkButton ID="btnStepSix" runat="server" CssClass="btn btn-sm  btn-success" OnClick="btnStepSix_Click" >Finish</asp:LinkButton>
                        <% } else
                            {%>
                        <%--<asp:LinkButton ID="LinkButton3"  title="Choose Departments & Common Designations" runat="server"  CssClass="btn btn-sm  btn-grey" OnClick="btnStepThree_Click" >Department, Designations</asp:LinkButton>--%>
                        <asp:LinkButton ID="LinkButton4" runat="server" title="Choose Employees" CssClass="btn btn-sm  btn-grey" OnClick="btnStepFour_Click" >Employees</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-sm  btn-success" title="Generate Reports" OnClick="btnStepSix_Click" >Finish</asp:LinkButton>
                        <% }%>   
                        <% }%> 
                        <%--<% }%> --%>
                        <hr />
                    </div>
                    <div style="margin-top:10px;">
                        <rsweb:ReportViewer AsyncRendering="false" ID="ReportViewer1" Height="1500px" Width="1000px" SizeToReportContent="true" runat="server"></rsweb:ReportViewer>
                         <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="56000"></asp:ScriptManager>
                     </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>


