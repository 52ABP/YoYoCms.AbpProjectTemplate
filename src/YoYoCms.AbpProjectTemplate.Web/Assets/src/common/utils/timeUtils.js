import * as typeUtils from './typeUtils'

/**
 * 获取当前时区与utc时间的间隔
 * @returns {number} 返回单位:秒
 */
export function getTimezonOffset() {
    return new Date().getTimezoneOffset() * 60
}

/**
 * 获取当前的时间戳 单位:秒
 */
export function getUTCTimetamp() {
    return parseInt(Date.now() / 1000) + getTimezonOffset()
}

/**
 * 日期转为字符串
 * @param time 时间戳获取是Date对象
 * @param isUTC 是否是UTC时间
 * @param withoutDate 是否不包含日期 只有时间
 */
export function time2String(time, withoutDate = false, isUTC) {
    //  如果time是日期类型
    if (typeUtils.isDate(time)) {
        // 转换为时间戳格式 并将单位转为秒
        time = time.getTime() / 1000
    }

    //  如果是utc时间 加上本地时区的差值
    if (isUTC) {
        time += getTimezonOffset()
    }

    //  最后再转为日期格式
    let finalDate = new Date(time * 1000)
    if (withoutDate) {
        //  返回字符串
        return fillZero(finalDate.getHours(), 2) + ':' + fillZero(finalDate.getMinutes(), 2)
    } else {
        //  返回字符串
        return finalDate.getFullYear() + '-' + (finalDate.getMonth() + 1) + '-' + finalDate.getDate() +
            ' ' + fillZero(finalDate.getHours(), 2) + ':' + fillZero(finalDate.getMinutes(), 2)
    }
}

/**
 * 获取时间间隔的描述字符串
 * @param time 时间戳
 * @param isUTC 是否是UTC时间(true表示北京时间减去8小时) 默认:true
 */
export function getTimespanDesc(time, isUTC = true) {
    // 当前的时间戳
    let currTimetamp = isUTC ? getUTCTimetamp() : Date.now()
    let timespan = currTimetamp / 1000 - time // 单位:秒

    //  1分钟内
    if (timespan < 60) {
        return '刚刚'
    }

    timespan = timespan / 60 // 单位:分
    //  1小时内
    if (timespan < 60) {
        return parseInt(timespan) + ' 分钟前'
    }

    timespan = timespan / 60 // 单位:时

    let monthSpan = new Date(currTimetamp).getMonth() - new Date(time * 1000).getMonth()

    // 判断你是否在同一个月
    if (timespan < 48 && monthSpan < 1) {
        //  一天还是两天内
        let daySpan = new Date(currTimetamp).getDate() - new Date(time * 1000).getDate()

        if (daySpan < 1) {        // 一天内
            return '今天 ' + time2String(time, true)
        } else if (daySpan < 2) { // 两天内
            return '昨天 ' + time2String(time, true)
        }
    }

    // 如果日期超过了两天(不在一个月或不在一年) 则直接显示创建时间
    return time2String(time, false, isUTC)
}

// 在字符串前面填充0
export function fillZero(orignStr, maxLength = 2) {
    orignStr = orignStr + '' // 将非字符串转为字符串
    let zeroCount = maxLength - orignStr.length
    let zeroStr = ''
    for (let i = 0; i < zeroCount; i++) {
        zeroStr = zeroStr + '0'
    }

    return zeroStr + orignStr
}

// 日期格式化 格式 例如：2017-02-22
export function getNowDate(isHour = 0, tamp = Date.now() / 1000) {
    let newDate = new Date(tamp * 1000)
    if (isHour) {
        return fillZero(newDate.getHours()) + ':' + fillZero(newDate.getMinutes())
    } else {
        return newDate.getFullYear() + '-' + fillZero(newDate.getMonth() + 1) + '-' + fillZero(newDate.getDate())
    }
}

// 获取星期几
export function getWeek(tamp) {
    let day = new Date(tamp).getDay()
    let weekDays = ['日', '一', '二', '三', '四', '五', '六']
    return weekDays[day]
}

/**
 * 日期转字符串
 * @param date
 * @param split 日期分割方式 默认 -
 * @param hasHour 是否需要转时分秒
 * @param hasDay 是否有日
 */
export function date2Str(date, split = '-', {hasHour = false, hasDay = true} = {}) {
    if (!typeUtils.isDate(date)) {
        return date
    }

    let ret = [date.getFullYear(), fillZero((date.getMonth() + 1), 2)]
    if (hasDay) {
        ret.push(fillZero(date.getDate(), 2))
    }

    let hours = ''

    // 如果有时分秒
    if (hasHour) {
        hours = `${fillZero(date.getHours())}:${fillZero(date.getMinutes())}:${fillZero(date.getSeconds())}`
    }

    ret = ret.join(split) + ' ' + hours
    return ret
}

/**
 * 重置日期的时分秒
 * @param date
 * @param type 时间的开始或结束 (start|end)
 */
export function resetDateTime(date, type = 'start') {
    // 2017-06-28T23:59:59.999Z
    let hms = type === 'end' ? 'T23:59:59.999Z' : 'T00:00:00.000Z'
    return date2Str(date) + hms
}

// 比较两个日期的大小(不比较具体的时间)
// 传入的参数必须是 Date 类型
export function compareDate(date1, date2) {
    let years = [date1.getFullYear(), date2.getFullYear()]
    let months = [date1.getMonth(), date2.getMonth()]
    let dates = [date1.getDate(), date2.getDate()]

    if (years[0] < years[1]) {
        return -1
    } else if (months[0] < months[1]) {
        return -1
    } else if (dates[0] < dates[1]) {
        return -1
    } else if (years[0] == years[1] && months[0] == months[1] && dates[0] == dates[1]) {
        return 0
    } else {
        return 1
    }
}

// 判断是否是闰年
export function leapYear(year) {
    return !(year % (year % 100 ? 4 : 400))
}

// 将日期加一天
export function addDay(date, addCount) {
    return new Date(date.setDate(date.getDate() + addCount))
}
