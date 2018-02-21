//为了查询 月份 统一而写此方法
function queryMonth(eventNode)
{
	var parm='';
	if(eventNode)
	{
	    //借助 prototype来准确定位，因为，滚动条可能让 日历定位不准
		var proEventObj=$(eventNode); 
		//获得文本框的绝对 x,y坐标
		var posObj=proEventObj.positionedOffset();
		//获得文本框的 高度
		var Height=proEventObj.getHeight();
		//获取 窗口滚动条的 x,y坐标
		var scrollOff = document.viewport.getScrollOffsets(); 
		//日历 弹出的实际纵坐标 =文本框的纵坐标+文本框的高度-滚动条的纵坐标
		posObj.top=posObj.top+Height-scrollOff.top;
		//日历 弹出的实际横坐标 =文本框的横坐标-横向滚动条的横做标
		posObj.left=posObj.left-scrollOff.left;
		//构造最后传给日历的参数
		parm={dateFmt:'yyyy年MM月',position:posObj};
	}
	else
	{
		parm={dateFmt:'yyyy年MM月'};
	}
    WdatePicker(parm);
}

/********************
 * 取窗口滚动条高度 
 ******************/
function getScrollTop()
{
    var scrollTop=0;
    if(document.documentElement&&document.documentElement.scrollTop)
    {
        scrollTop=document.documentElement.scrollTop;
    }
    else if(document.body)
    {
        scrollTop=document.body.scrollTop;
    }
    return scrollTop;
}


/********************
 * 取窗口可视范围的高度 
 *******************/
function getClientHeight()
{
    var clientHeight=0;
    if(document.body.clientHeight&&document.documentElement.clientHeight)
    {
        var clientHeight = (document.body.clientHeight<document.documentElement.clientHeight)?document.body.clientHeight:document.documentElement.clientHeight;        
    }
    else
    {
        var clientHeight = (document.body.clientHeight>document.documentElement.clientHeight)?document.body.clientHeight:document.documentElement.clientHeight;    
    }
    return clientHeight;
}

/********************
 * 取文档内容实际高度 
 *******************/
function getScrollHeight()
{
    return Math.max(document.body.scrollHeight,document.documentElement.scrollHeight);
}