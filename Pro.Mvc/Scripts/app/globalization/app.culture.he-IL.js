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
