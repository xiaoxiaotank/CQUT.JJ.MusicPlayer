
$(function () {
    var mapChart = echarts.init(document.getElementById('mapDiv'));
    var newPublishChart = echarts.init(document.getElementById('newPublishDiv'));
    var newUnpublishChart = echarts.init(document.getElementById('newUnpublishDiv'));


    $.getScript('http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js', function () {
        if (remote_ip_info.ret == '1') {
            //alert('国家：' + remote_ip_info.country + '<BR>省：' + remote_ip_info.province + '<BR>市：' + remote_ip_info.city + '<BR>区：' + remote_ip_info.district + '<BR>ISP：' + remote_ip_info.isp + '<BR>类型：' + remote_ip_info.type + '<BR>其他：' + remote_ip_info.desc);
            onlineData = [{
                name: remote_ip_info.province,
                selected: true,
                itemStyle: itemSelectedStyle
            }];
        }

        var mapOption = {
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
        mapChart.setOption(mapOption);
    });    
        

    var newPublishOption = {

        title: {
            text: '今日发布信息饼状图',
            left: 'center',
            top: 20,
            textStyle: {
                color: '#ccc'
            }
        },

        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },

        visualMap: {
            show: false,
            min: 0,
            max: 100,
            inRange: {
                colorLightness: [0, 1]
            }
        },
        series: [
            {
                name: '发布数量',
                type: 'pie',
                radius: '55%',
                center: ['50%', '50%'],
                data: [
                    { value: 10, name: '歌唱家' },
                    { value: 9, name: '专辑' },
                    { value: 37, name: '音乐' },
                ],/*.sort(function (a, b) { return a.value - b.value; }),*/
                roseType: 'radius',
                label: {
                    normal: {
                        textStyle: {
                            color: 'rgba(255, 255, 255, 0.6)'
                        }
                    }
                },
                labelLine: {
                    normal: {
                        lineStyle: {
                            color: 'rgba(255, 255, 255, 0.6)'
                        },
                        smooth: 0.2,
                        length: 10,
                        length2: 20
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#c23531',
                        shadowBlur: 200,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                },

                animationType: 'scale',
                animationEasing: 'elasticOut',
                animationDelay: function (idx) {
                    return Math.random() * 200;
                }
            }
        ]
    };
    newPublishChart.setOption(newPublishOption);

    var newUnpublishOption = {

        title: {
            text: '今日下架信息饼状图',
            left: 'center',
            top: 20,
            textStyle: {
                color: '#ccc'
            }
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        visualMap: {
            show: false,
            min: 0,
            max: 100,
            inRange: {
                colorLightness: [0, 1]
            }
        },
        series: [
            {
                name: '下架数量',
                type: 'pie',
                radius: '55%',
                center: ['50%', '50%'],
                data: [
                    { value: 20, name: '歌唱家' },
                    { value: 39, name: '专辑' },
                    { value: 37, name: '音乐' },
                ],
                roseType: 'radius',
                label: {
                    normal: {
                        textStyle: {
                            color: 'rgba(255, 255, 255, 0.6)'
                        }
                    }
                },
                labelLine: {
                    normal: {
                        lineStyle: {
                            color: 'rgba(255, 255, 255, 0.6)'
                        },
                        smooth: 0.2,
                        length: 10,
                        length2: 20
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#c23531',
                        shadowBlur: 200,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                },

                animationType: 'scale',
                animationEasing: 'elasticOut',
                animationDelay: function (idx) {
                    return Math.random() * 200;
                }
            }
        ]
    };
    newUnpublishChart.setOption(newUnpublishOption);
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