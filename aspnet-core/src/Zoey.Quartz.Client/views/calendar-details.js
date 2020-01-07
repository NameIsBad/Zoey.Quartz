"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var aurelia_framework_1 = require("aurelia-framework");
var aurelia_router_1 = require("aurelia-router");
var aurelia_http_client_1 = require("aurelia-http-client");
var toastr = require("toastr");
var bootbox = require("bootbox");
var CalendarDetailsView = (function () {
    function CalendarDetailsView(router, http) {
        this.router = router;
        this.http = http;
    }
    CalendarDetailsView.prototype.activate = function (params) {
        this.schedulerName = params.schedulerName;
        this.name = params.name;
        return this.loadDetails();
    };
    CalendarDetailsView.prototype.delete = function () {
        var _this = this;
        bootbox.confirm({
            size: "small",
            message: "Delete " + this.name + "?",
            callback: function (result) {
                if (result) {
                    return _this.http.delete("/api/schedulers/" + _this.schedulerName + "/calendars/" + _this.name).then(function () {
                        toastr.success("Calendar " + _this.name + " deleted successfully");
                        return _this.router.navigate("calendars");
                    });
                }
                return $.when();
            }
        });
    };
    CalendarDetailsView.prototype.loadDetails = function () {
        var _this = this;
        return this.http.get("/api/schedulers/" + this.schedulerName + "/calendars/" + this.name).then(function (response) {
            _this.details = response.content;
        });
    };
    CalendarDetailsView.prototype.isAnnualCalendar = function () {
        return this.calendarTypeNameContains("AnnualCalendar");
    };
    CalendarDetailsView.prototype.isCronCalendar = function () {
        return this.calendarTypeNameContains("CronCalendar");
    };
    CalendarDetailsView.prototype.isDailyCalendar = function () {
        return this.calendarTypeNameContains("DailyCalendar");
    };
    CalendarDetailsView.prototype.isHolidayCalendar = function () {
        return this.calendarTypeNameContains("HolidayCalendar");
    };
    CalendarDetailsView.prototype.isMonthlyCalendar = function () {
        return this.calendarTypeNameContains("MonthlyCalendar");
    };
    CalendarDetailsView.prototype.isWeeklyCalendar = function () {
        return this.calendarTypeNameContains("WeeklyCalendar");
    };
    CalendarDetailsView.prototype.calendarTypeNameContains = function (name) {
        return this.details && this.details.calendarType.indexOf(name) > -1;
    };
    var _a, _b;
    CalendarDetailsView = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [typeof (_a = typeof aurelia_router_1.Router !== "undefined" && aurelia_router_1.Router) === "function" ? _a : Object, typeof (_b = typeof aurelia_http_client_1.HttpClient !== "undefined" && aurelia_http_client_1.HttpClient) === "function" ? _b : Object])
    ], CalendarDetailsView);
    return CalendarDetailsView;
}());
exports.CalendarDetailsView = CalendarDetailsView;
//# sourceMappingURL=calendar-details.js.map