<%@ Page Language="vb" %>
<%@ import Namespace="System.Configuration" %>
<%@ import Namespace="System.Web.UI.WebControls" %>
<%@ import Namespace="System" %>
<script runat="server">

    Private Sub Page_Load(sender As Object, e As System.EventArgs)
       If Not Page.IsPostBack Then
          Dim selected As String = Request.QueryString("selected")
          Dim id As String = Request.QueryString("id")
          Dim form As String = Request.QueryString("formname")
          Dim postBack As String = Request.QueryString("postBack")
    
          Cal.FirstDayofWeek = 1
    
          ' Select the Correct date for Calendar from query string
          ' If fails, pick the current date on Calendar
          Try
             Cal.SelectedDate = CDate(selected)
             Cal.VisibleDate = Cal.SelectedDate
          Catch
              Cal.SelectedDate = DateTime.Today
             Cal.VisibleDate = Cal.SelectedDate
          End Try
    
          ' Fills in correct values for the dropdown menus
          FillCalendarChoices()
          SelectCorrectValues()
    
          ' Add JScript to the OK button so that when the user clicks on it, the selected date
          ' is passed back to the calling page.
          OKButton.Attributes.Add("onClick", "window.opener.SetDate('" + form + "','" + id + "', document.Calendar.datechosen.value," + postBack + ");")
          CancelButton.Attributes.Add("onClick", "CloseWindow()")
       End If
    End Sub 'Page_Load
    
    
    '***************************************************************
    '
    ' FillCalendarChoices method is used to fill dropdowns with month and year values
    '
    '***************************************************************
    Private Sub FillCalendarChoices()
       Dim thisdate As New DateTime(DateTime.Today.Year, 1, 1)
    
       ' Fills in month values
       Dim x As Integer
       For x = 0 To 11
          ' Loops through 12 months of the year and fills in each month value
          Dim li As New ListItem(thisdate.ToString("MMMM"), thisdate.Month.ToString())
          MonthSelect.Items.Add(li)
          thisdate = thisdate.AddMonths(1)
       Next x
    
       ' Fills in year values and change y value to other years if necessary
       Dim y As Integer
       For y = 1994 To thisdate.Year
          YearSelect.Items.Add(y.ToString())
       Next y
    End Sub 'FillCalendarChoices
    
    '***************************************************************************
    '
    ' The SelectCorrectValues method is used to select the correct values in dropdowns
    ' according to the selected date on Calendar
    '
    '***************************************************************************
    
    Private Sub SelectCorrectValues()
       lblDate.Text = Cal.SelectedDate.ToShortDateString()
       datechosen.Value = lblDate.Text
       MonthSelect.SelectedIndex = MonthSelect.Items.IndexOf(MonthSelect.Items.FindByValue(Cal.SelectedDate.Month.ToString()))
       YearSelect.SelectedIndex = YearSelect.Items.IndexOf(YearSelect.Items.FindByValue(Cal.SelectedDate.Year.ToString()))
    End Sub 'SelectCorrectValues
    
    '**************************************************************************
    '
    ' Cal_SelectionChanged event handler highlights the selected date on the Calendar and
    ' calls SelectCorrectValues() to synchronize to correct values on dropdowns.
    '
    '**************************************************************************
    
    Private Sub Cal_SelectionChanged(sender As Object, e As System.EventArgs)
       Cal.VisibleDate = Cal.SelectedDate
       SelectCorrectValues()
    End Sub 'Cal_SelectionChanged
    
    '**************************************************************************
    '
    ' MonthSelect_SelectedIndexChanged event handler selects the first day of the month when
    ' a month selection has being changed.
    '
    '**************************************************************************
    
    Private Sub MonthSelect_SelectedIndexChanged(sender As Object, e As System.EventArgs)
       Cal.VisibleDate = New DateTime(Convert.ToInt32(YearSelect.SelectedItem.Value), Convert.ToInt32(MonthSelect.SelectedItem.Value), 1)
       Cal.SelectedDate = Cal.VisibleDate
       SelectCorrectValues()
    End Sub 'MonthSelect_SelectedIndexChanged
    
    '**************************************************************************
    '
    ' YearSelect_SelectedIndexChanged event handler selects a year value on the Calendar control
    ' when a year selection has being changed.
    '
    '**************************************************************************
    
    Private Sub YearSelect_SelectedIndexChanged(sender As Object, e As System.EventArgs)
       Cal.VisibleDate = New DateTime(Convert.ToInt32(YearSelect.SelectedItem.Value), Convert.ToInt32(MonthSelect.SelectedItem.Value), 1)
       Cal.SelectedDate = Cal.VisibleDate
       SelectCorrectValues()
    End Sub 'YearSelect_SelectedIndexChanged

</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Calendar</title> 
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="styles.css" type="text/css" rel="stylesheet" />
    <script language="javascript">
            function CloseWindow()
            {
                self.close();
            }
        </script>
</head>
<body bgcolor="#ffffff" leftmargin="5" topmargin="5">
    <form id="Calendar" method="post" runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tbody>
                <tr bgcolor="white">
                    <td colspan="2">
                        <img height="10" src="images/spacer.gif" width="1" /></td>
                </tr>
                <tr bgcolor="white">
                    <td align="middle" colspan="2">
                        <asp:dropdownlist id="MonthSelect" runat="server" OnSelectedIndexChanged="MonthSelect_SelectedIndexChanged" AutoPostBack="True" Width="90px" Height="22px" CssClass="standard-text"></asp:dropdownlist>
                        &nbsp; 
                        <asp:dropdownlist id="YearSelect" runat="server" OnSelectedIndexChanged="YearSelect_SelectedIndexChanged" AutoPostBack="True" Width="60px" Height="22px" CssClass="standard-text"></asp:dropdownlist>
                        <asp:calendar id="Cal" runat="server" CssClass="standard-text" OnSelectionChanged="Cal_SelectionChanged" FirstDayOfWeek="Monday" ForeColor="#C0C0FF" DayNameFormat="FirstTwoLetters" BorderColor="White" Font-Names="Arial" Font-Size="XX-Small" BorderStyle="Solid" ShowNextPrevMonth="False" ShowTitle="False" BorderWidth="5px">
                            <todaydaystyle font-bold="True" forecolor="White" backcolor="#990000"></todaydaystyle>
                            <daystyle borderwidth="2px" forecolor="#666666" borderstyle="Solid" bordercolor="White" backcolor="#EAEAEA"></daystyle>
                            <dayheaderstyle forecolor="#649CBA"></dayheaderstyle>
                            <selecteddaystyle font-bold="True" forecolor="#333333" backcolor="#FAAD50"></selecteddaystyle>
                            <weekenddaystyle forecolor="White" backcolor="#BBBBBB"></weekenddaystyle>
                            <othermonthdaystyle forecolor="#666666" backcolor="White"></othermonthdaystyle>
                        </asp:calendar>
                    </td>
                </tr>
                <tr>
                    <td align="middle" colspan="2">
                        Date Selected: <asp:Label id="lblDate" runat="server"></asp:Label>
                        <input id="datechosen" type="hidden" name="datechosen" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <img height="10" src="images/spacer.gif" width="1" /></td>
                </tr>
                <tr>
                    <td align="middle">
                        <asp:button id="OKButton" runat="server" Width="60px" CssClass="btn" Text="OK"></asp:button>
                    </td>
                    <td align="middle">
                        <a href="javascript:CloseWindow()">
                        <asp:button id="CancelButton" runat="server" Width="60px" CssClass="btn" Text="Cancel"></asp:button>
                        </a></td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>