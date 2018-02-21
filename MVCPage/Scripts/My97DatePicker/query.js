function queryMonth(eventNode)
{
	var parm='';
	if(eventNode)
	{
		var proEventObj=$(eventNode); 
		var posObj=proEventObj.positionedOffset();
		var Height=proEventObj.getHeight();
		
		var scrollOff = document.viewport.getScrollOffsets(); 
		posObj.top=posObj.top+Height-scrollOff.top;
		posObj.left=posObj.left-scrollOff.left;
		parm={dateFmt:'yyyy年MM月',position:posObj};
	}
	else
	{
		parm={dateFmt:'yyyy年MM月'};
	}
    WdatePicker(parm);
}