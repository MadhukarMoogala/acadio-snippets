### Substitute BigFont in Text Style
**Workitem Json**
```json
{
  "Arguments": {
    "InputArguments": [
      {
        "Resource": "https://s3-us-west-2.amazonaws.com/xiaodongforgetestio/Example+-+Copy.dwg",
        "Name": "HostDwg"
      },
      {
        "Resource": "https://madhukar-fda.s3.us-west-2.amazonaws.com/pdfgen.scr",
        "Name": "MyScript"
      }
    ],
    "OutputArguments": [
      {
        "Name": "MyResult",
        "HttpVerb": "POST"
      }
    ]
  },
  "ActivityId": "BigFontSubst"
}
```
**Activity Json**
```json
{
  "Id": "BigFontSubst",
  "Instruction": {
    "Script": "_.SCRIPTCALL\npdfgen.scr\n_layoutcreateviewport 1\n_tilemode 0\n-export _pdf _all result.pdf\n"
  },
  "Parameters": {
    "InputParameters": [
      {
        "LocalFileName": "$(HostDwg)",
        "Name": "HostDwg"
      },
      {
        "LocalFileName": "pdfgen.scr",
        "Name": "MyScript"
      }
    ],
    "OutputParameters": [
      {
        "LocalFileName": "result.pdf",
        "Name": "MyResult"
      }
    ]
  },
  "RequiredEngineVersion": "23.0"
}
```
**Contents of pdfgen.scr**
```
;Iterates through all style records, if font is of shape file
;substitutes bigFont with known @extfont2.shx
(while (setq sty (tblnext "style" (not sty)))
	(if (= (wcmatch (cdr (assoc 3 sty)) "*.shx") T)
	  (progn
        	(setq enx (entget (tblobjname "style" (cdr (assoc 2 sty)))))
        	(entmod (subst '(4 . "@extfont2.shx") (assoc 4 enx) enx))
	  )
	)
)
;Regenerating Model for style effects to takeplace
REGEN
```