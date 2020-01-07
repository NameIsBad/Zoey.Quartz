"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function configure(aurelia) {
    aurelia.use
        .standardConfiguration()
        .developmentLogging()
        .feature("resources");
    aurelia.start().then(function (a) { return a.setRoot("views/app"); });
}
exports.configure = configure;
//# sourceMappingURL=main.js.map