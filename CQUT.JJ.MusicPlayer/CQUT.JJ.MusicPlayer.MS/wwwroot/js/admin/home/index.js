
$(function () {
    var mapChart = echarts.init(document.getElementById('mapDiv'));
    var newPublishChart = echarts.init(document.getElementById('newPublishDiv'));
    var newUnpublishChart = echarts.init(document.getElementById('newUnpublishDiv'));
    var recentPublishChart = echarts.init(document.getElementById('recentPublishDiv'));

    $.getScript('http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js', function () {
        if (remote_ip_info.ret == '1') {
            //alert('国家：' + remote_ip_info.country + '<BR>省：' + remote_ip_info.province + '<BR>市：' + remote_ip_info.city + '<BR>区：' + remote_ip_info.district + '<BR>ISP：' + remote_ip_info.isp + '<BR>类型：' + remote_ip_info.type + '<BR>其他：' + remote_ip_info.desc);
            onlineData = [{
                name: remote_ip_info.province,
                selected: true,
                itemStyle: itemSelectedStyle
            }];
            onlinePosition = remote_ip_info.province + remote_ip_info.city + remote_ip_info.district;
        }
        else {
            onlineData = [{
                name: "重庆",
                selected: true,
                itemStyle: itemSelectedStyle
            }];
            onlinePosition = "重庆";
        }

        var mapOption = {
            tooltip: {
                show: true,
                formatter: function (params) {
                    if (params.data != null && params.data.selected == true) {
                        return '在线位置:' + onlinePosition;//提示标签格式
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
        
    //请求今日上下架信息
    $.ajax({
        type: 'get',
        url: '/Admin/Home/GetPublishedInfoPerDay',
        data: { "dayNumber": 1 },
        success: function (result) {
            var dataInfo = [];
            $.each(result, function (i, r) {
                if (r.value === null) {
                    dataInfo.push({ value:0, name:r.name });
                } else {
                    var valueInfo = 0;
                    for (var v in r.value) {
                        valueInfo = r.value[v];
                    }
                    dataInfo.push({ value: valueInfo, name:r.name })
                }
            })
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
                        data: dataInfo,/*.sort(function (a, b) { return a.value - b.value; }),*/
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
        }
    })

    //请求近一周上架信息
    $.ajax({
        type: 'get',
        url: '/Admin/Home/GetPublishedInfoPerDay',
        data: { "dayNumber": 7 },
        success: function (result) {
            var dataInfo = [["最近一周上架信息表", result[0].name, result[1].name, result[2].name]];
            for (var r in result[0].value) {
                if (r === null) {
                    return;
                } else {
                    dataInfo.push([ r.substr(0, 10), result[0].value[r], result[1].value[r], result[2].value[r] ]);
                }
            }
            

            var recentPublishOption = {
                legend: {},
                tooltip: {},
                dataset: {
                    source: dataInfo
                },
                xAxis: { type: 'category' },
                yAxis: {},
                series: [
                    { type: 'bar' },
                    { type: 'bar' },
                    { type: 'bar' }
                ]
            };
            recentPublishChart.setOption(recentPublishOption);
        }
    })

    //请求用户数量信息
    $.ajax({
        type: 'get',
        url: "/Admin/Home/GetUserCount",
        success: function (data) {
            $("#memberCount").text(data.memberCount);
            $("#memberRegisterCount").text(data.todayRegisterMemberCount);
            $("#employeeCount").text(data.employeeCount);
            $("#employeeRegisterCount").text(data.todayCreateEmployeeCount);
        }
    })
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
var onlinePosition;