<%@ Page Language="VB" %>
<script runat="server">

    Sub Button_Action(Sender as object, e as eventargs)
    
    app_message.innerhtml = "Start Date is " & TextBox1.text & "<BR>" & "End Date is " & Textbox2.text
    
    
    End Sub

</script>
<html>
<head>
    <title>Popup Calendar Example</title> <script language="javascript" src="script.js" type="text/javascript"></script>
</head>
<body>
    <form id="testform" runat="server">
        <p>
            Enter Start Date<asp:TextBox id="TextBox1" runat="server"></asp:TextBox>
            <a href="javascript:OpenCalendar('TextBox1', false)"><img height="16" src="icon-calendar.gif" width="24" align="absMiddle" border="0" /></a> 
        </p>
        <p>
            Enter End Date<asp:TextBox id="TextBox2" runat="server"></asp:TextBox>
            <a href="javascript:OpenCalendar('TextBox2', false)"><img height="16" src="icon-calendar.gif" width="24" align="absMiddle" border="0" /></a> 
        </p>
        <p>
            <asp:Button id="Button1" onclick="Button_Action" runat="server" Text="Submit"></asp:Button>
        </p>
        <div id="app_message" runat="server">
        </div>
    </form>
</body>
</html>
