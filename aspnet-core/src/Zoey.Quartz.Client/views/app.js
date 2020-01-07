"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var App = (function () {
    function App() {
    }
    App.prototype.configureRouter = function (config, router) {
        config.title = "Quartz Web Console";
        config.map([
            { route: ["", "dashboard"], moduleId: "views/dashboard", nav: true, title: "Dashboard" },
            { route: ["schedulers/:schedulerName"], name: "scheduler-details", moduleId: "views/scheduler-router", nav: false, title: "Scheduler Details" }
        ]);
        this.router = router;
    };
    return App;
}());
exports.App = App;
//# sourceMappingURL=app.js.map