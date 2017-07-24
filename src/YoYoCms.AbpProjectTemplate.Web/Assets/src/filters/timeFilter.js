/**
 * Created by huanghx on 2017/6/26.
 */
import Vue from 'vue'
import moment from 'moment'

import * as typeUtils from '../common/utils/typeUtils'
function loadLocation() {
    if (abp.localization.currentLanguage.name === 'zh-CN')
        require('moment/locale/zh-cn')
}

Vue.filter('date2str', function (date, hasHour) {
    loadLocation()
    if (!date) return ''
    if (typeUtils.isString(date)) date = new Date(date)
    let format = 'YYYY-MM-DD'
    if (hasHour) format += ' HH:MM:SS'
    return moment(date).format(format)
})

Vue.filter('date2calendar', function (date) {
    loadLocation()
    return moment(date).calendar()
})