/*
 * app.Globalize Culture he-IL
 */

(function( window, undefined ) {

var Globalize;

if ( typeof require !== "undefined" &&
	typeof exports !== "undefined" &&
	typeof module !== "undefined" ) {
	// Assume CommonJS
	Globalize = require( "globalize" );
} else {
	// Global variable
	Globalize = window.Globalize;
}

Globalize.addCultureInfo( "he-IL", "default", {
	name: "he-IL",
	englishName: "Hebrew (Israel)",
	nativeName: "עברית (ישראל)",
	language: "he",
	isRTL: true,
	numberFormat: {
		"NaN": "לא מספר",
		negativeInfinity: "אינסוף שלילי",
		positiveInfinity: "אינסוף חיובי",
		percent: {
			pattern: ["-n%","n%"]
		},
		currency: {
			pattern: ["$-n","$ n"],
			symbol: "₪"
		}
	},
	global:{
	    "/": " ",
	    incorrectValue: "הערך שהוקלד אינו תקין",
	    dataNotFound: "לא נמצאו נתונים",
	    loadtext: "Loading...",
	    clearstring: "נקה",
	    todaystring: "היום",
	    search: "איתור"
	},
    tasks:{
        noteIsRequired: "נא לציין הערה",
        confirmStartTask: "האם לאתחל משימה?",
        openTasks: 'משימות ממתינות',
        activeTasks: 'משימות פעילות',
        closedTasks: 'משימות סגורות',
        todayTasks: 'משימות להיום',
        tasks: 'משימות',
        task: 'משימה',
        ticket: 'כרטיס',
        reminder: 'תזכורות',
        calendar: 'יומן'

	}
});

}( this ));



/*
!function (a, b) {
    var c;
    c = "undefined" != typeof require && "undefined" != typeof exports && "undefined" != typeof module ? require("globalize") : a.Globalize,
    c.addCultureInfo("he-IL", "default", {
        name: "he-IL", englishName: "Hebrew (Israel)", nativeName: "עברית (ישראל)", language: "he", isRTL: !0,
        numberFormat: {
            NaN: "לא מספר",
            negativeInfinity: "אינסוף שלילי",
            positiveInfinity: "אינסוף חיובי",
            percent: { pattern: ["-n%", "n%"] },
            currency: { pattern: ["$-n", "$ n"], symbol: "₪" }
        }, calendars: {
            standard: {
                days: {
                    names: ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "יום שישי", "שבת"],
                    namesAbbr: ["יום א", "יום ב", "יום ג", "יום ד", "יום ה", "יום ו", "שבת"],
                    namesShort: ["א", "ב", "ג", "ד", "ה", "ו", "ש"]
                }, months: {
                    names: ["ינואר", "פברואר", "מרץ", "אפריל", "מאי", "יוני", "יולי", "אוגוסט", "ספטמבר", "אוקטובר", "נובמבר", "דצמבר", ""],
                    namesAbbr: ["ינו", "פבר", "מרץ", "אפר", "מאי", "יונ", "יול", "אוג", "ספט", "אוק", "נוב", "דצמ", ""]
                }, eras: [{ name: "לספירה", start: null, offset: 0 }],
                patterns: { d: "dd/MM/yyyy", D: "dddd dd MMMM yyyy", t: "HH:mm", T: "HH:mm:ss", f: "dddd dd MMMM yyyy HH:mm", F: "dddd dd MMMM yyyy HH:mm:ss", M: "dd MMMM", Y: "MMMM yyyy" }
            }, Hebrew: {
                name: "Hebrew", "/": " ", days: {
                    names: ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "יום שישי", "שבת"],
                    namesAbbr: ["א", "ב", "ג", "ד", "ה", "ו", "ש"],
                    namesShort: ["א", "ב", "ג", "ד", "ה", "ו", "ש"]
                }, months: {
                    names: ["תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר ב", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול"],
                    namesAbbr: ["תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "אדר ב", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול"]
                }, eras: [{ name: "C.E.", start: null, offset: 0 }],
                twoDigitYearMax: 5790,
                patterns: { d: "dd MMMM yyyy", D: "dddd dd MMMM yyyy", t: "HH:mm", T: "HH:mm:ss", f: "dddd dd MMMM yyyy HH:mm", F: "dddd dd MMMM yyyy HH:mm:ss", M: "dd MMMM", Y: "MMMM yyyy" }
            }
        }
    })
}(this);
*/