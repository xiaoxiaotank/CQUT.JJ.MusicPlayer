
$(function () {
    $.getScript('http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js', function () {
        if (remote_ip_info.ret == '1') {
            //alert('国家：' + remote_ip_info.country + '<BR>省：' + remote_ip_info.province + '<BR>市：' + remote_ip_info.city + '<BR>区：' + remote_ip_info.district + '<BR>ISP：' + remote_ip_info.isp + '<BR>类型：' + remote_ip_info.type + '<BR>其他：' + remote_ip_info.desc);
            onlineData = [{
                name: remote_ip_info.province,
                selected: true,
                itemStyle: itemSelectedStyle
            }];
        }

        var option = {
            tooltip: {
                show: true,
                formatter: function (params) {
                    if (params.data != null && params.data.selected == true) {
                        return '在线位置:' + params.data.name;//提示标签格式
                    }
                },
                backgroundColor: labelStyle.emphasis.backgroundColor,
                textStyle: labelStyle.emphasis.textStyle,
                padding: labelStyle.emphasis.padding
            },
            series: [{
                type: 'map',
                mapType: 'china',
                roam: true,
                label: labelStyle,
                itemStyle: itemStyle,
                data: onlineData
            }],
        };
        mapChart.setOption(option);
    });    
        
    var mapChart = echarts.init(document.getElementById('mapDiv'));
    
    
})

//hover时样式
var labelStyle = {
    emphasis:{
        show: true,
        padding: 4,
        textStyle: {
            color: "white",
            fontSize: 13,
        },
        backgroundColor: 'orange'
    }    
};

var itemStyle = {
    normal: {
        areaColor: '#D2D6DE', //区域颜色
        borderColor: 'white'  //边界颜色
    },
    emphasis: {
        areaColor: '#E2E0E8'  //hover时区域颜色
    }
};

var itemSelectedStyle = {
    normal: {
        borderColor: 'black'  //边界颜色
    },
    emphasis: {
        areaColor: '#B13533'  //hover时区域颜色
    }
};

var onlineData;