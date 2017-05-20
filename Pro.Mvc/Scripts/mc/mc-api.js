
function apiContactQuerySync(uarg, parg, qt, args, count) {
    var response;
    alert(13);
    $.support.cors = true;
    $.ajax({
        //Access-Control-Request-Method : "POST",
        //Access-Control-Request-Headers: "X-PINGOTHER",
        type: "POST",
        url: "http://api.my-t.co.il:9008/Services/ApiJs.asmx/ApiQuery",
        //data: "{ 'UArg' : '"+uarg+"', 'PArg' : '"+parg+"', 'QueryType' : '"+qt+"', 'Args' : '"+args+"', 'Count' : "+count+" }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        crossDomain: true,
        async: false,
        data: JSON.stringify("{ 'UArg' : '" + uarg + "', 'PArg' : '" + parg + "', 'QueryType' : '" + qt + "', 'Args' : '" + args + "', 'Count' : " + count + " }"),
        processData: false,

        beforeSend: function (xhr) {
            //xhr.setRequestHeader('Authority', '*');
            xhr.setRequestHeader('X-PINGOTHER', 'pingpong');
            xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
        },
        error: function (xhr, status, err) {
            alert("An error occurred: " + err);
        },
        success: function (xhr) {
            //alert("success" + xhr);

            response = xhr;

        }
    });
    //alert(response);
    return response;
}

