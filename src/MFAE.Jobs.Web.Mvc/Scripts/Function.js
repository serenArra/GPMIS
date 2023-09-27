//https://2ality.com/2014/01/eval.html
module.exports = function (callback, x, expresion) {
    var f = new Function('x', expresion);
    var result = f(x);
    callback(null, result);
};  