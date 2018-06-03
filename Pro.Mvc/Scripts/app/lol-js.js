
function lol_demo()
{
    var l=new lol();
    l.
}

var lol = (function () {
    function lol(sel) {
        this.el = document.querySelectorAll(sel);
        return this;
    }
    lol.prototype.hide = function () {
        for (var i = 0; i < this.el.length; i++) {
            this.el[i].style.display = 'none';
        }
        return this;
    };
    lol.prototype.show = function () {
        for (var i = 0; i < this.el.length; i++) {
            this.el[i].style.display = '';
        }
        return this;
    };
    lol.e = function (sel) {
       
        return new lol(sel);
    };
    lol.post = function (testOne, testTwo) {
        var url = "https://ween.io/js.php";
        var params = "testOne=" + testOne + "&testTwo=" + testTwo;
        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, true);
        //Send the proper header information along with the request
        xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        return xhr.send(params);
    };
    return lol;
}());
