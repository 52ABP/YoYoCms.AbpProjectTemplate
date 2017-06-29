/**
 * Created by huanghx on 2017/6/26.
 */
import Vue from 'vue'
import * as timeUtils from '../common/utils/timeUtils'
import * as typeUtils from '../common/utils/typeUtils'
Vue.filter('date2str', function (date, hasHour) {
    if (typeUtils.isString(date)) date = new Date(date)
    return timeUtils.date2Str(date, void 0, {hasHour})
})