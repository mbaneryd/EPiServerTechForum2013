function init(){SXE.initElementDialog("ins"),"update"==SXE.currentAction&&(setFormValue("datetime",tinyMCEPopup.editor.dom.getAttrib(SXE.updateElement,"datetime")),setFormValue("cite",tinyMCEPopup.editor.dom.getAttrib(SXE.updateElement,"cite")),SXE.showRemoveButton())}function setElementAttribs(elm){setAllCommonAttribs(elm),setAttrib(elm,"datetime"),setAttrib(elm,"cite"),elm.removeAttribute("data-mce-new")}function insertIns(){var elm=tinyMCEPopup.editor.dom.getParent(SXE.focusElement,"INS");if(null==elm){var s=SXE.inst.selection.getContent();if(s.length>0){insertInlineElement("ins");for(var elementArray=SXE.inst.dom.select("ins[data-mce-new]"),i=0;elementArray.length>i;i++){var elm=elementArray[i];setElementAttribs(elm)}}}else setElementAttribs(elm);tinyMCEPopup.editor.nodeChanged(),tinyMCEPopup.execCommand("mceEndUndoLevel"),tinyMCEPopup.close()}function removeIns(){SXE.removeElement("ins"),tinyMCEPopup.close()}tinyMCEPopup.onInit.add(init);