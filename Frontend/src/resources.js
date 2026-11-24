const text = (key, label, required = true, group) => ({ key, label, type: 'text', required, group });
const number = (key, label, required = true, group) => ({ key, label, type: 'number', required, group });
const relation = (key, label, source, required = true, group) => ({ key, label, type: 'relation', source, required, group });
const choice = (key, label, options, required = true, group) => ({ key, label, type: 'choice', options, required, group });
const longText = (key, label, required = false, group) => ({ key, label, type: 'textarea', required, group });
const date = (key, label, required = true, group) => ({ key, label, type: 'date', required, group });

export const RESOURCE_GROUPS = [
  { id: 'directory', label: 'Основные справочники', resources: ['organizations', 'suborganizations', 'employees', 'positions', 'employeePositions', 'students'] },
  { id: 'study', label: 'Учебный процесс', resources: ['studyPrograms', 'studyPlans', 'studyGroups', 'disciplines', 'fgosDisciplines', 'fgosDisciplineMappings', 'studyPlanDisciplines'] },
  { id: 'performance', label: 'Успеваемость', resources: ['importBatches', 'academicPerformanceRecords', 'studentDisciplineGrades'] },
  { id: 'licenses', label: 'Лицензии', resources: ['licenses', 'studentLicenses', 'studentLicenseHistories'] },
  { id: 'attestation', label: 'Аттестации', resources: ['attestationCommissions', 'attestationCommissionMembers'] },
];

export const RESOURCES = {
  organizations: {
    group: 'directory', route: 'organizations', endpoint: '/api/organizations', label: 'Организации', singular: 'организацию', description: 'Организации, к которым относятся подразделения.',
    fields: [text('name', 'Название')], columns: [['name', 'Название']],
  },
  suborganizations: {
    group: 'directory', route: 'suborganizations', endpoint: '/api/suborganizations', label: 'Подразделения', singular: 'подразделение', description: 'Подразделения организаций.',
    fields: [text('name', 'Название'), relation('organizationId', 'Организация', 'organizations')], columns: [['name', 'Подразделение'], ['organizationId', 'ID организации']],
  },
  employees: {
    group: 'directory', route: 'employees', endpoint: '/api/employees', label: 'Сотрудники', singular: 'сотрудника', description: 'Сотрудники и преподаватели.',
    fields: [text('fullName', 'ФИО')], columns: [['fullName', 'ФИО']],
  },
  positions: {
    group: 'directory', route: 'positions', endpoint: '/api/positions', label: 'Должности', singular: 'должность', description: 'Список должностей сотрудников.',
    fields: [text('title', 'Название должности')], columns: [['title', 'Должность']],
  },
  employeePositions: {
    group: 'directory', route: 'employee-positions', endpoint: '/api/employee-positions', label: 'Должности сотрудников', singular: 'связь', description: 'Назначения должностей сотрудникам.',
    fields: [relation('employeeId', 'Сотрудник', 'employees'), relation('positionId', 'Должность', 'positions')], columns: [['employeeFullName', 'Сотрудник'], ['positionTitle', 'Должность']],
  },
  students: {
    group: 'directory', route: 'students', endpoint: '/api/students', label: 'Студенты', singular: 'студента', description: 'Студенты и сведения о поступлении.',
    fields: [text('email', 'Email'), text('lastName', 'Фамилия'), text('firstName', 'Имя'), text('patronymic', 'Отчество', false), choice('admissionBase', 'База поступления', [['9', 'После 9 класса'], ['11', 'После 11 класса']], false), number('admissionYear', 'Год поступления', false)],
    columns: [['lastName', 'Фамилия'], ['firstName', 'Имя'], ['patronymic', 'Отчество'], ['email', 'Email'], ['admissionBase', 'База'], ['admissionYear', 'Год поступления']],
  },
  studyPrograms: {
    group: 'study', route: 'study-programs', endpoint: '/api/studyprograms', label: 'Образовательные программы', singular: 'программу', description: 'Программы, квалификации и коды специальностей.',
    fields: [text('name', 'Название'), text('qualification', 'Квалификация'), text('specialtyCode', 'Код специальности'), relation('suborganizationId', 'Подразделение', 'suborganizations')], columns: [['name', 'Программа'], ['qualification', 'Квалификация'], ['specialtyCode', 'Код специальности'], ['suborganizationId', 'ID подразделения']],
  },
  studyPlans: {
    group: 'study', route: 'study-plans', endpoint: '/api/study-plans', label: 'Учебные планы', singular: 'учебный план', description: 'Учебные планы и параметры потока.',
    fields: [choice('admissionBase', 'База поступления', [['9', 'После 9 класса'], ['11', 'После 11 класса']]), number('admissionYear', 'Год поступления'), text('name', 'Название', false), text('sourceFileName', 'Исходный файл', false), relation('studyProgramId', 'Образовательная программа', 'studyPrograms')], columns: [['name', 'Учебный план'], ['admissionBase', 'База'], ['admissionYear', 'Год поступления'], ['studyProgramId', 'ID программы'], ['sourceFileName', 'Исходный файл']],
  },
  studyGroups: {
    group: 'study', route: 'study-groups', endpoint: '/api/study-groups', label: 'Учебные группы', singular: 'группу', description: 'Группы и их связи с подразделениями и учебными планами.',
    fields: [text('name', 'Название группы'), relation('suborganizationId', 'Подразделение', 'suborganizations'), relation('studyProgramId', 'Образовательная программа', 'studyPrograms', false), relation('studyPlanId', 'Учебный план', 'studyPlans', false)], columns: [['name', 'Группа'], ['suborganizationId', 'ID подразделения'], ['studyProgramId', 'ID программы'], ['studyPlanId', 'ID учебного плана']],
  },
  disciplines: {
    group: 'study', route: 'disciplines', endpoint: '/api/disciplines', label: 'Дисциплины', singular: 'дисциплину', description: 'Дисциплины учебных подразделений.',
    fields: [text('code', 'Индекс'), text('name', 'Название'), relation('suborganizationId', 'Подразделение', 'suborganizations')], columns: [['code', 'Индекс'], ['name', 'Дисциплина'], ['suborganizationId', 'ID подразделения']],
  },
  fgosDisciplines: {
    group: 'study', route: 'fgos-disciplines', endpoint: '/api/fgos-disciplines', label: 'Дисциплины ФГОС', singular: 'дисциплину ФГОС', description: 'Нормативный перечень дисциплин ФГОС.',
    fields: [text('code', 'Код'), text('name', 'Название'), relation('studyProgramId', 'Образовательная программа', 'studyPrograms')], columns: [['code', 'Код'], ['name', 'Дисциплина'], ['studyProgramId', 'ID программы']],
  },
  fgosDisciplineMappings: {
    group: 'study', route: 'fgos-discipline-mappings', endpoint: '/api/fgos-discipline-mappings', label: 'Сопоставления ФГОС', singular: 'сопоставление', description: 'Связи дисциплин учебного плана с дисциплинами ФГОС.',
    fields: [relation('disciplineId', 'Дисциплина', 'disciplines'), relation('fgosDisciplineId', 'Дисциплина ФГОС', 'fgosDisciplines')], columns: [['disciplineCode', 'Код дисциплины'], ['disciplineName', 'Дисциплина'], ['fgosDisciplineCode', 'Код ФГОС'], ['fgosDisciplineName', 'Дисциплина ФГОС']],
  },
  studyPlanDisciplines: {
    group: 'study', route: 'study-plan-disciplines', endpoint: '/api/study-plan-disciplines', label: 'Дисциплины учебных планов', singular: 'дисциплину плана', description: 'Дисциплины, семестры и плановые часы учебных планов.',
    fields: [relation('studyPlanId', 'Учебный план', 'studyPlans'), relation('fgosDisciplineId', 'Дисциплина ФГОС', 'fgosDisciplines'), number('semesterNumber', 'Семестр'), number('plannedHours', 'Плановые часы', false), text('controlForm', 'Форма контроля', false)], columns: [['studyPlanName', 'Учебный план'], ['fgosDisciplineCode', 'Код'], ['fgosDisciplineName', 'Дисциплина'], ['semesterNumber', 'Семестр'], ['plannedHours', 'Часы'], ['controlForm', 'Контроль']],
  },
  importBatches: {
    group: 'performance', route: 'import-batches', endpoint: '/api/import-batches', label: 'Партии импорта', singular: 'партию', description: 'Импортированные файлы и количество обработанных строк.',
    fields: [date('importedAt', 'Дата импорта'), number('rowCount', 'Количество строк'), text('semester', 'Семестр', false), text('sourceFileName', 'Имя файла')],
    createFields: [text('sourceFileName', 'Имя файла'), number('rowCount', 'Количество строк'), text('semester', 'Семестр', false)], columns: [['importedAt', 'Дата импорта'], ['sourceFileName', 'Файл'], ['semester', 'Семестр'], ['rowCount', 'Строк']],
  },
  academicPerformanceRecords: {
    group: 'performance', route: 'academic-performance-records', endpoint: '/api/academic-performance-records', label: 'Записи успеваемости', singular: 'запись успеваемости', description: 'Строки импортированных ведомостей и итоговые баллы.',
    fields: [
      relation('importBatchId', 'Партия импорта', 'importBatches', true, 'Источник'), number('sourceRowNumber', 'Номер строки', true, 'Источник'),
      relation('studentId', 'Студент', 'students', true, 'Учебные данные'), relation('disciplineId', 'Дисциплина', 'disciplines', true, 'Учебные данные'), number('semesterId', 'ID семестра', true, 'Учебные данные'), relation('studyGroupId', 'Учебная группа', 'studyGroups', true, 'Учебные данные'), relation('organizationId', 'Организация', 'organizations', true, 'Учебные данные'), relation('suborganizationId', 'Подразделение', 'suborganizations', true, 'Учебные данные'),
      number('finalPoints', 'Итоговые баллы', true, 'Баллы'), number('finalPointsFivePointScale', 'Оценка по пятибалльной шкале', true, 'Баллы'), number('currentPointsFivePointScale', 'Текущие баллы по пятибалльной шкале', true, 'Баллы'), number('availableControlPoints', 'Доступные контрольные баллы', true, 'Баллы'), number('maxAvailableControlPoints', 'Максимум контрольных баллов', true, 'Баллы'), number('maxDisciplinePoints', 'Максимум баллов за дисциплину', true, 'Баллы'), number('maxPointsByTopics', 'Максимум баллов по темам', true, 'Баллы'), number('taskAndTestPoints', 'Баллы за задания и тесты', true, 'Баллы'), number('retakePoints', 'Баллы пересдачи', false, 'Баллы'), number('retakePointsNew', 'Баллы новой пересдачи', false, 'Баллы'),
      text('currentDisciplinePercent', 'Процент по дисциплине', false, 'Дополнительно'), text('currentStudentAveragePercent', 'Средний процент студента', false, 'Дополнительно'), longText('allDisciplineTeachers', 'Все преподаватели дисциплины', false, 'Дополнительно'), longText('teachersFromWorkload', 'Преподаватели из нагрузки', false, 'Дополнительно'), longText('teachersWhoGraded', 'Преподаватели, выставившие оценку', false, 'Дополнительно'),
    ],
    columns: [['studentId', 'ID студента'], ['disciplineId', 'ID дисциплины'], ['semesterId', 'ID семестра'], ['finalPoints', 'Итоговые баллы'], ['finalPointsFivePointScale', 'Оценка'], ['sourceRowNumber', 'Строка файла']],
  },
  studentDisciplineGrades: {
    group: 'performance', route: 'student-discipline-grades', endpoint: '/api/student-discipline-grades', label: 'Оценки студентов', singular: 'оценку', description: 'Оценки по дисциплинам с преподавателем и датой.',
    fields: [relation('studentId', 'Студент', 'students'), relation('disciplineId', 'Дисциплина', 'disciplines'), relation('employeeId', 'Преподаватель', 'employees'), relation('academicPerformanceId', 'Запись успеваемости', 'academicPerformanceRecords', false), number('admissionYear', 'Год поступления'), number('semesterNumber', 'Семестр'), text('grade', 'Оценка'), date('date', 'Дата')],
    columns: [['studentId', 'ID студента'], ['disciplineId', 'ID дисциплины'], ['employeeId', 'ID преподавателя'], ['grade', 'Оценка'], ['date', 'Дата'], ['semesterNumber', 'Семестр']],
  },
  licenses: {
    group: 'licenses', route: 'licenses', endpoint: '/api/licenses', label: 'Лицензии', singular: 'лицензию', description: 'Справочник типов лицензий.',
    fields: [text('name', 'Название')], columns: [['name', 'Лицензия']],
  },
  studentLicenses: {
    group: 'licenses', route: 'student-licenses', endpoint: '/api/student-licenses', label: 'Лицензии студентов', singular: 'лицензию студента', description: 'Выданные студентам лицензии.',
    fields: [relation('studentId', 'Студент', 'students'), relation('licenseId', 'Лицензия', 'licenses')], columns: [['studentFullName', 'Студент'], ['licenseName', 'Лицензия']],
  },
  studentLicenseHistories: {
    group: 'licenses', route: 'student-license-histories', endpoint: '/api/student-license-histories', label: 'История лицензий', singular: 'запись истории', description: 'История действия лицензий по семестрам.',
    fields: [relation('studentLicenseId', 'Лицензия студента', 'studentLicenses'), text('academicYear', 'Учебный год', false), number('semesterNumber', 'Номер семестра', false), text('semesterLabel', 'Подпись семестра')],
    columns: [['studentFullName', 'Студент'], ['licenseName', 'Лицензия'], ['academicYear', 'Учебный год'], ['semesterNumber', 'Семестр'], ['semesterLabel', 'Подпись']],
  },
  attestationCommissions: {
    group: 'attestation', route: 'attestation-commissions', endpoint: '/api/attestation-commissions', label: 'Аттестационные комиссии', singular: 'комиссию', description: 'Комиссии, программы, семестры и связанные группы.',
    fields: [relation('studyProgramId', 'Образовательная программа', 'studyPrograms'), relation('disciplineId', 'Дисциплина', 'disciplines', false), relation('studyGroupId', 'Учебная группа', 'studyGroups', false), text('academicYear', 'Учебный год', false), text('semesterLabel', 'Подпись семестра', false), number('semesterNumber', 'Номер семестра', false)],
    columns: [['studyProgramName', 'Программа'], ['disciplineName', 'Дисциплина'], ['studyGroupName', 'Группа'], ['academicYear', 'Учебный год'], ['semesterLabel', 'Семестр']],
  },
  attestationCommissionMembers: {
    group: 'attestation', route: 'attestation-commission-members', endpoint: '/api/attestation-commission-members', label: 'Состав комиссий', singular: 'участника комиссии', description: 'Участники, должности и роли в комиссиях.',
    fields: [relation('commissionId', 'Комиссия', 'attestationCommissions'), text('fullName', 'ФИО'), text('position', 'Должность', false), text('role', 'Роль'), number('sortOrder', 'Порядок отображения')],
    columns: [['commissionId', 'ID комиссии'], ['fullName', 'ФИО'], ['position', 'Должность'], ['role', 'Роль'], ['sortOrder', 'Порядок']],
  },
};

// GET filters supported by the API. Field keys must match the query parameter names.
export const RESOURCE_FILTERS = {
  organizations: [text('name', 'Название', false)],
  suborganizations: [text('name', 'Название', false), relation('organizationId', 'Организация', 'organizations', false)],
  employees: [text('fullName', 'ФИО', false)],
  positions: [text('title', 'Должность', false)],
  employeePositions: [
    relation('employeeId', 'Сотрудник', 'employees', false), relation('positionId', 'Должность', 'positions', false),
    text('employeeName', 'ФИО сотрудника', false), text('positionTitle', 'Название должности', false),
  ],
  studyPrograms: [
    text('name', 'Название программы', false), text('specialtyCode', 'Код специальности', false),
    relation('suborganizationId', 'Подразделение', 'suborganizations', false),
  ],
  studyPlans: [
    text('name', 'Название плана', false), choice('admissionBase', 'База поступления', [['9', 'После 9 класса'], ['11', 'После 11 класса']], false),
    number('admissionYear', 'Год поступления', false), relation('studyProgramId', 'Образовательная программа', 'studyPrograms', false),
  ],
  disciplines: [
    text('code', 'Индекс', false), text('name', 'Название', false), relation('suborganizationId', 'Подразделение', 'suborganizations', false),
  ],
  fgosDisciplines: [
    text('name', 'Название', false), text('code', 'Код', false), relation('studyProgramId', 'Образовательная программа', 'studyPrograms', false),
  ],
  fgosDisciplineMappings: [
    relation('disciplineId', 'Дисциплина', 'disciplines', false), relation('fgosDisciplineId', 'Дисциплина ФГОС', 'fgosDisciplines', false),
    text('disciplineCode', 'Индекс дисциплины', false), text('disciplineName', 'Название дисциплины', false),
    text('fgosDisciplineCode', 'Код дисциплины ФГОС', false), text('fgosDisciplineName', 'Название дисциплины ФГОС', false),
  ],
  studyPlanDisciplines: [
    relation('studyPlanId', 'Учебный план', 'studyPlans', false), relation('fgosDisciplineId', 'Дисциплина ФГОС', 'fgosDisciplines', false),
    number('semesterNumber', 'Семестр', false), text('controlForm', 'Форма контроля', false),
    text('fgosDisciplineCode', 'Код дисциплины ФГОС', false), text('fgosDisciplineName', 'Название дисциплины ФГОС', false),
  ],
  importBatches: [
    text('semester', 'Семестр', false), text('sourceFileName', 'Имя файла', false), date('importedFrom', 'Импортировано с', false),
    date('importedTo', 'Импортировано по', false), number('rowCount', 'Количество строк', false),
  ],
  studentLicenses: [
    relation('studentId', 'Студент', 'students', false), relation('licenseId', 'Лицензия', 'licenses', false),
    text('studentName', 'ФИО студента', false), text('licenseName', 'Название лицензии', false),
  ],
  studentLicenseHistories: [
    relation('studentLicenseId', 'Лицензия студента', 'studentLicenses', false), relation('studentId', 'Студент', 'students', false),
    relation('licenseId', 'Лицензия', 'licenses', false), text('academicYear', 'Учебный год', false),
    number('semesterNumber', 'Номер семестра', false), text('semesterLabel', 'Подпись семестра', false),
    text('studentName', 'ФИО студента', false), text('licenseName', 'Название лицензии', false),
  ],
  attestationCommissions: [
    relation('disciplineId', 'Дисциплина', 'disciplines', false), relation('studyGroupId', 'Учебная группа', 'studyGroups', false),
    relation('studyProgramId', 'Образовательная программа', 'studyPrograms', false), text('academicYear', 'Учебный год', false),
    text('semesterLabel', 'Подпись семестра', false), number('semesterNumber', 'Номер семестра', false),
    text('disciplineName', 'Название дисциплины', false), text('studyGroupName', 'Название группы', false),
  ],
  attestationCommissionMembers: [
    relation('commissionId', 'Комиссия', 'attestationCommissions', false), text('fullName', 'ФИО', false),
    text('position', 'Должность', false), text('role', 'Роль', false), number('sortOrder', 'Порядок отображения', false),
  ],
};

export const RESOURCE_BY_ROUTE = Object.fromEntries(Object.entries(RESOURCES).map(([id, resource]) => [resource.route, id]));

export function displayValue(row, key) {
  const value = row?.[key];
  if (value === null || value === undefined || value === '') return '—';
  if ((key === 'date' || key === 'importedAt') && !Number.isNaN(Date.parse(value))) {
    return new Intl.DateTimeFormat('ru-RU', { dateStyle: 'medium' }).format(new Date(value));
  }
  if (key === 'admissionBase') return value === 9 ? 'После 9 класса' : value === 11 ? 'После 11 класса' : value;
  return String(value);
}

export function optionLabel(row, source) {
  const labelKeys = {
    organizations: ['name'], suborganizations: ['name'], employees: ['fullName'], positions: ['title'], students: ['lastName', 'firstName', 'patronymic'],
    studyPrograms: ['name', 'specialtyCode'], studyPlans: ['name', 'admissionYear'], studyGroups: ['name'], disciplines: ['code', 'name'], fgosDisciplines: ['code', 'name'],
    importBatches: ['sourceFileName', 'semester'], academicPerformanceRecords: ['id', 'sourceRowNumber'], licenses: ['name'], studentLicenses: ['studentFullName', 'licenseName'],
    attestationCommissions: ['id', 'semesterLabel'], attestationCommissionMembers: ['fullName', 'role'],
  }[source] || ['id'];
  const label = labelKeys.map((key) => row?.[key]).filter(Boolean).join(' · ');
  return label || `Запись #${row?.id ?? ''}`;
}
