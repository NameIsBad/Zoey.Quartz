"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DurationFormatValueConverter = (function () {
    function DurationFormatValueConverter() {
    }
    DurationFormatValueConverter.prototype.toView = function (value, format) {
        if (!value) {
            return "";
        }
        return moment.utc(moment.duration(value).asMilliseconds()).format("HH:mm:ss.SSS");
    };
    return DurationFormatValueConverter;
}());
exports.DurationFormatValueConverter = DurationFormatValueConverter;
//# sourceMappingURL=duration-format.js.map