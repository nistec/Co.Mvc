﻿var getLocalization = function (culture) {
    var localization = null;
    switch (culture) {
        case "he":
            localization =
             {
                 // separator of parts of a date (e.g. '/' in 11/05/1955)
                 '/': "/",
                 // separator of parts of a time (e.g. ':' in 05:44 PM)
                 ':': ":",
                 // the first day of the week (0 = Sunday, 1 = Monday, etc)
                 firstDay: 1,
                 days: {
                     names: ["יום ראשון", "יום שני", "יום שלישי", "יום רביעי", "יום חמישי", "יום שישי", "שבת"],
                     namesAbbr: ["יום א", "יום ב", "יום ג", "יום ד", "יום ה", "יום ו", "שבת"],
                     namesShort: ["א", "ב", "ג", "ד", "ה", "ו", "ש"]
                 },
                 months: {
                     names: ["ינואר", "פברואר", "מרץ", "אפריל", "מאי", "יוני", "יולי", "אוגוסט", "ספטמבר", "אוקטובר", "נובמבר", "דצמבר", ""],
                     namesAbbr: ["ינו", "פבר", "מרץ", "אפר", "מאי", "יונ", "יול", "אוג", "ספט", "אוק", "נוב", "דצמ", ""]
                 },
                 // AM and PM designators in one of these forms:
                 // The usual view, and the upper and lower case versions
                 //      [standard,lowercase,uppercase]
                 // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
                 //      null
                 AM: ["AM", "am", "AM"],
                 PM: ["PM", "pm", "PM"],
                 eras: [{ "name": "לספירה", "start": null, "offset": 0 }],
                 twoDigitYearMax: 2029,
                 patterns: {
                     d: "dd/MM/yyyy",
                     D: "dddd dd MMMM yyyy",
                     t: "HH:mm",
                     T: "HH:mm:ss",
                     f: "dddd dd MMMM yyyy HH:mm",
                     F: "dddd dd MMMM yyyy HH:mm:ss",
                     M: "dd MMMM",
                     Y: "MMMM yyyy"
                 },
                 percentsymbol: "%",
                 currencysymbol: "$",
                 currencysymbolposition: "לפני",
                 decimalseparator: '.',
                 thousandsseparator: ',',
                 pagergotopagestring: "הצג עמוד:",
                 pagershowrowsstring: "הצג שורות:",
                 pagerrangestring: " מתוך ",
                 pagerpreviousbuttonstring: "הקודם",
                 pagernextbuttonstring: "הבא",
                 pagerfirstbuttonstring: "ראשון",
                 pagerlastbuttonstring: "אחרון",
                 groupsheaderstring: "Drag a column and drop it here to group by that column",
                 sortascendingstring: "מיון בסדר עולה",
                 sortdescendingstring: "מיון בסדר יורד",
                 sortremovestring: "ביטול מיון",
                 groupbystring: "קבץ לפי עמודה",
                 groupremovestring: "הסר קיבוץ",
                 filterclearstring: "נקה",
                 filterstring: "סינון",
                 filtershowrowstring: "הצג רשומות היכן ש:",//"Show rows where:",
                 filterorconditionstring: "או",
                 filterandconditionstring: "וגם",
                 filterselectallstring: "(הצג הכל)",
                 filterchoosestring: "נא לבחור:",
                 //filterstringcomparisonoperators: ['empty', 'not empty', 'enthalten', 'enthalten(match case)',
                 //   'does not contain', 'does not contain(match case)', 'starts with', 'starts with(match case)',
                 //   'ends with', 'ends with(match case)', 'equal', 'equal(match case)', 'null', 'not null'],
                 filterstringcomparisonoperators: ['ריק', 'לא ריק', 'מכיל', 'מכיל(רישיות)',
                    'אינו מכיל', 'אינו מכיל(רישיות)', 'מתחיל ב', 'מתחיל ב(רישיות)',
                    'מסתיים ב', 'מסתיים ב(רישיות)', 'שווה', 'שווה(רישיות)', 'null', 'not null'],
                 //filternumericcomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
                 filternumericcomparisonoperators: ['שווה', 'שונה', 'קטן מ', 'קטן או שווה', 'גדול מ', 'גדול או שווה', 'ריק', 'לא ריק'],
                 //filterdatecomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
                 filterdatecomparisonoperators: ['שווה', 'שונה', 'קטן מ', 'קטן או שווה', 'גדול מ', 'גדול או שווה', 'ריק', 'לא ריק'],
                 filterbooleancomparisonoperators: ['equal', 'not equal'],
                 //validationstring: "Entered value is not valid",
                 validationstring: "הערך שהוקלד אינו תקין",
                 //emptydatastring: "No data to display",
                 emptydatastring: "לא נמצאו נתונים",
                 //filterselectstring: "Select Filter",
                 filterselectstring: "בחירת סינון",
                 loadtext: "Loading...",
                 clearstring: "נקה",
                 todaystring: "היום"
             }
            break;
        case "en":
        default:
            localization =
            {
                // separator of parts of a date (e.g. '/' in 11/05/1955)
                '/': "/",
                // separator of parts of a time (e.g. ':' in 05:44 PM)
                ':': ":",
                // the first day of the week (0 = Sunday, 1 = Monday, etc)
                firstDay: 0,
                days: {
                    // full day names
                    names: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
                    // abbreviated day names
                    namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
                    // shortest day names
                    namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
                },
                months: {
                    // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
                    names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
                    // abbreviated month names
                    namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
                },
                // AM and PM designators in one of these forms:
                // The usual view, and the upper and lower case versions
                //      [standard,lowercase,uppercase]
                // The culture does not use AM or PM (likely all standard date formats use 24 hour time)
                //      null
                AM: ["AM", "am", "AM"],
                PM: ["PM", "pm", "PM"],
                eras: [
                // eras in reverse chronological order.
                // name: the name of the era in this culture (e.g. A.D., C.E.)
                // start: when the era starts in ticks (gregorian, gmt), null if it is the earliest supported era.
                // offset: offset in years from gregorian calendar
                { "name": "A.D.", "start": null, "offset": 0 }
                ],
                twoDigitYearMax: 2029,
                patterns: {
                    // short date pattern
                    d: "M/d/yyyy",
                    // long date pattern
                    D: "dddd, MMMM dd, yyyy",
                    // short time pattern
                    t: "h:mm tt",
                    // long time pattern
                    T: "h:mm:ss tt",
                    // long date, short time pattern
                    f: "dddd, MMMM dd, yyyy h:mm tt",
                    // long date, long time pattern
                    F: "dddd, MMMM dd, yyyy h:mm:ss tt",
                    // month/day pattern
                    M: "MMMM dd",
                    // month/year pattern
                    Y: "yyyy MMMM",
                    // S is a sortable format that does not vary by culture
                    S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss",
                    // formatting of dates in MySQL DataBases
                    ISO: "yyyy-MM-dd hh:mm:ss",
                    ISO2: "yyyy-MM-dd HH:mm:ss",
                    d1: "dd.MM.yyyy",
                    d2: "dd-MM-yyyy",
                    d3: "dd-MMMM-yyyy",
                    d4: "dd-MM-yy",
                    d5: "H:mm",
                    d6: "HH:mm",
                    d7: "HH:mm tt",
                    d8: "dd/MMMM/yyyy",
                    d9: "MMMM-dd",
                    d10: "MM-dd",
                    d11: "MM-dd-yyyy"
                },
                percentsymbol: "%",
                currencysymbol: "$",
                currencysymbolposition: "before",
                decimalseparator: '.',
                thousandsseparator: ',',
                pagergotopagestring: "Go to page:",
                pagershowrowsstring: "Show rows:",
                pagerrangestring: " of ",
                pagerpreviousbuttonstring: "previous",
                pagernextbuttonstring: "next",
                pagerfirstbuttonstring: "first",
                pagerlastbuttonstring: "last",
                groupsheaderstring: "Drag a column and drop it here to group by that column",
                sortascendingstring: "Sort Ascending",
                sortdescendingstring: "Sort Descending",
                sortremovestring: "Remove Sort",
                groupbystring: "Group By this column",
                groupremovestring: "Remove from groups",
                filterclearstring: "Clear",
                filterstring: "Filter",
                filtershowrowstring: "Show rows where:",
                filterorconditionstring: "Or",
                filterandconditionstring: "And",
                filterselectallstring: "(Select All)",
                filterchoosestring: "Please Choose:",
                filterstringcomparisonoperators: ['empty', 'not empty', 'enthalten', 'enthalten(match case)',
                   'does not contain', 'does not contain(match case)', 'starts with', 'starts with(match case)',
                   'ends with', 'ends with(match case)', 'equal', 'equal(match case)', 'null', 'not null'],
                filternumericcomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
                filterdatecomparisonoperators: ['equal', 'not equal', 'less than', 'less than or equal', 'greater than', 'greater than or equal', 'null', 'not null'],
                filterbooleancomparisonoperators: ['equal', 'not equal'],
                validationstring: "Entered value is not valid",
                emptydatastring: "No data to display",
                filterselectstring: "Select Filter",
                loadtext: "Loading...",
                clearstring: "Clear",
                todaystring: "Today"
            }
            break;
    }
    return localization;
}