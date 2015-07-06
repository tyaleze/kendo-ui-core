(function(f, define){
    define([
        "./spreadsheet/rangelist",
        "./spreadsheet/references",
        "./spreadsheet/range",
        "./spreadsheet/sheet",
        "./spreadsheet/formulacontext",
        "./spreadsheet/view",
        "./spreadsheet/grid",
        "./spreadsheet/axis",
        "./spreadsheet/sorter",
        "./spreadsheet/runtime",
        "./spreadsheet/calc",
        "./spreadsheet/numformat",
        "./spreadsheet/runtime.functions.js"
    ], f);
})(function(){
    var __meta__ = {
        id: "spreadsheet",
        name: "Spreadsheet",
        category: "web",
        description: "Spreadsheet component",
        depends: [],
        features: []
    };

    var Widget = kendo.ui.Widget;

    var Spreadsheet = kendo.ui.Widget.extend({
        init: function(element, options) {
            Widget.fn.init.call(this, element, options);

            this.element.addClass("k-widget k-spreadsheet");

            this._view = new kendo.spreadsheet.View(this.element);

            this._sheet = new kendo.spreadsheet.Sheet(
                this.options.rows,
                this.options.columns,
                this.options.rowHeight,
                this.options.columnWidth,
                this.options.headerHeight,
                this.options.headerWidth
            );

            this._autoRefresh = true;

            this._sheet.bind("change", function(e) {
                if (this._autoRefresh) {
                    this.refresh(e);
                }
            }.bind(this));

            var context = {};

            this._sheet.name("Sheet1");

            context[this._sheet.name()] = this._sheet;

            this._context = new kendo.spreadsheet.FormulaContext(context);

            this._view.sheet(this.activeSheet());

            this.fromJSON(this.options);
        },

        refresh: function(e) {
            if (!e || e.recalc === true) {
                this._sheet.recalc(this._context);
            }
            this._view.refresh();
            this._view.render();
            this.trigger("render");
            return this;
        },

        autoRefresh: function(value) {
            if (value !== undefined) {
                this._autoRefresh = value;

                if (value === true) {
                    this.refresh();
                }

                return this;
            }

            return this._autoRefresh;
        },

        activeSheet: function() {
            return this._sheet;
        },

        toJSON: function() {
            return {
                sheets: [this._sheet].map(function(sheet) {
                    sheet.recalc(this._context);
                    return sheet.toJSON();
                }, this)
            };
        },

        fromJSON: function(json) {
            if (json.sheets) {
                this._sheet.fromJSON(json.sheets[0]);
            }
        },

        options: {
            name: "Spreadsheet",
            rows: 200,
            columns: 50,
            rowHeight: 20,
            columnWidth: 64,
            headerHeight: 20,
            headerWidth: 32
        },

        events: [
            "render"
        ]
    });

    kendo.ui.plugin(Spreadsheet);
}, typeof define == 'function' && define.amd ? define : function(_, f){ f(); });
