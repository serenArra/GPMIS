module.exports = function (callback, x, expresion) {
    let result = eval(expresion);
    callback(null, result);
};  