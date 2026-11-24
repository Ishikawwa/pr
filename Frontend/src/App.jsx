import { useCallback, useEffect, useMemo, useState } from 'react';
import { RESOURCE_BY_ROUTE, RESOURCE_FILTERS, RESOURCE_GROUPS, RESOURCES, displayValue, optionLabel } from './resources.js';

async function request(url, options) {
  const response = await fetch(url, options);
  const raw = await response.text();
  let payload = null;
  if (raw) {
    try { payload = JSON.parse(raw); } catch { payload = raw; }
  }
  if (!response.ok) {
    const validation = payload?.errors && Object.values(payload.errors).flat().join(' ');
    const message = Array.isArray(payload) ? payload.join(' ')
      : payload?.detail || validation || payload?.title || payload?.message || payload || `Ошибка запроса (${response.status})`;
    throw new Error(String(message));
  }
  return payload;
}

function resourceFromPath() {
  return RESOURCE_BY_ROUTE[location.pathname.split('/').filter(Boolean)[0]] || 'students';
}

function App() {
  const [resourceId, setResourceId] = useState(resourceFromPath);
  const [rows, setRows] = useState([]);
  const [loading, setLoading] = useState(true);
  const [pageError, setPageError] = useState('');
  const [search, setSearch] = useState('');
  const [sectionSearch, setSectionSearch] = useState('');
  const [filterValues, setFilterValues] = useState({});
  const [appliedFilters, setAppliedFilters] = useState({});
  const [filterOptions, setFilterOptions] = useState({});
  const [filterOptionsLoading, setFilterOptionsLoading] = useState(false);
  const [dialog, setDialog] = useState(null);
  const [saving, setSaving] = useState(false);
  const [options, setOptions] = useState({});
  const [optionsLoading, setOptionsLoading] = useState(false);

  const resource = RESOURCES[resourceId];
  const filters = RESOURCE_FILTERS[resourceId] || [];
  const hasAppliedFilters = Object.values(appliedFilters).some((value) => value !== '' && value !== null && value !== undefined);
  const loadRows = useCallback(async (id = resourceId, queryFilters = appliedFilters) => {
    setLoading(true);
    setPageError('');
    try {
      const query = new URLSearchParams();
      Object.entries(queryFilters || {}).forEach(([key, value]) => {
        if (value !== '' && value !== null && value !== undefined) query.set(key, value);
      });
      const queryString = query.toString();
      const suffix = queryString ? `?${queryString}` : '';
      const result = await request(`${RESOURCES[id].endpoint}${suffix}`);
      setRows(Array.isArray(result) ? result : []);
    } catch (error) {
      setPageError(error.message || 'Не удалось загрузить данные. Проверьте подключение к API.');
      setRows([]);
    } finally {
      setLoading(false);
    }
  }, [appliedFilters, resourceId]);

  useEffect(() => {
    const onPopState = () => {
      setResourceId(resourceFromPath());
      setSearch('');
      setFilterValues({});
      setAppliedFilters({});
      setDialog(null);
    };
    addEventListener('popstate', onPopState);
    loadRows(resourceId);
    return () => removeEventListener('popstate', onPopState);
  }, [loadRows, resourceId]);

  useEffect(() => {
    if (!dialog || dialog.mode !== 'edit') {
      setOptions({});
      return undefined;
    }
    const fields = dialog.item ? (resource.editFields || resource.fields) : (resource.createFields || resource.fields);
    const sourceIds = [...new Set(fields.filter((field) => field.type === 'relation').map((field) => field.source))];
    if (!sourceIds.length) {
      setOptions({});
      return undefined;
    }
    let active = true;
    setOptionsLoading(true);
    Promise.allSettled(sourceIds.map(async (sourceId) => {
      const data = await request(RESOURCES[sourceId].endpoint);
      return [sourceId, Array.isArray(data) ? data : []];
    })).then((results) => {
      if (active) {
        const fulfilled = results.filter((result) => result.status === 'fulfilled').map((result) => result.value);
        const rejected = results.filter((result) => result.status === 'rejected');
        setOptions(Object.fromEntries(fulfilled));
        setDialog((current) => current ? { ...current, optionsError: rejected[0]?.reason?.message || '' } : current);
      }
    }).finally(() => {
      if (active) setOptionsLoading(false);
    });
    return () => { active = false; };
  }, [dialog?.mode, dialog?.item?.id, resourceId]);

  useEffect(() => {
    const sourceIds = [...new Set((RESOURCE_FILTERS[resourceId] || [])
      .filter((field) => field.type === 'relation')
      .map((field) => field.source))];
    if (!sourceIds.length) {
      setFilterOptions({});
      setFilterOptionsLoading(false);
      return undefined;
    }
    let active = true;
    setFilterOptionsLoading(true);
    Promise.allSettled(sourceIds.map(async (sourceId) => {
      const data = await request(RESOURCES[sourceId].endpoint);
      return [sourceId, Array.isArray(data) ? data : []];
    })).then((results) => {
      if (!active) return;
      const fulfilled = results.filter((result) => result.status === 'fulfilled').map((result) => result.value);
      setFilterOptions(Object.fromEntries(fulfilled));
    }).finally(() => {
      if (active) setFilterOptionsLoading(false);
    });
    return () => { active = false; };
  }, [resourceId]);

  useEffect(() => {
    if (!dialog) return undefined;
    const onKeyDown = (event) => {
      if (event.key === 'Escape' && !saving) setDialog(null);
    };
    addEventListener('keydown', onKeyDown);
    return () => removeEventListener('keydown', onKeyDown);
  }, [dialog, saving]);

  const visibleRows = useMemo(() => {
    if (!search.trim()) return rows;
    const needle = search.trim().toLocaleLowerCase('ru');
    return rows.filter((row) => JSON.stringify(row).toLocaleLowerCase('ru').includes(needle));
  }, [rows, search]);

  const visibleGroups = useMemo(() => {
    const needle = sectionSearch.trim().toLocaleLowerCase('ru');
    if (!needle) return RESOURCE_GROUPS;
    return RESOURCE_GROUPS.map((group) => {
      const groupMatches = group.label.toLocaleLowerCase('ru').includes(needle);
      const resources = group.resources.filter((id) => groupMatches || RESOURCES[id].label.toLocaleLowerCase('ru').includes(needle));
      return { ...group, resources };
    }).filter((group) => group.resources.length > 0);
  }, [sectionSearch]);

  function navigate(event, id) {
    event.preventDefault();
    if (id === resourceId) return;
    history.pushState({}, '', `/${RESOURCES[id].route}`);
    setResourceId(id);
    setSearch('');
    setFilterValues({});
    setAppliedFilters({});
    setDialog(null);
  }

  function applyFilters(event) {
    event.preventDefault();
    setAppliedFilters({ ...filterValues });
  }

  function clearFilters() {
    setFilterValues({});
    setAppliedFilters({});
  }

  async function submitForm(event) {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const fields = dialog.item ? (resource.editFields || resource.fields) : (resource.createFields || resource.fields);
    const payload = Object.fromEntries(fields.map((field) => [field.key, parseField(formData.get(field.key), field)]));
    setSaving(true);
    setDialog((current) => ({ ...current, error: '', optionsError: '' }));
    try {
      const editing = Boolean(dialog.item);
      await request(`${resource.endpoint}${editing ? `/${dialog.item.id}` : ''}`, {
        method: editing ? 'PUT' : 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });
      setDialog(null);
      await loadRows(resourceId);
    } catch (error) {
      setDialog((current) => ({ ...current, error: error.message || 'Не удалось сохранить запись.' }));
    } finally {
      setSaving(false);
    }
  }

  async function deleteItem() {
    setSaving(true);
    try {
      await request(`${resource.endpoint}/${dialog.item.id}`, { method: 'DELETE' });
      setDialog(null);
      await loadRows(resourceId);
    } catch (error) {
      setDialog((current) => ({ ...current, error: error.message || 'Не удалось удалить запись.' }));
    } finally {
      setSaving(false);
    }
  }

  return (
    <main className="app-shell">
      <header className="topbar">
        <a className="brand" href="/students" onClick={(event) => navigate(event, 'students')} aria-label="IThub, открыть студентов">
          <span className="brand-mark" aria-hidden="true">i</span><span>IThub</span>
        </a>
        <span className="topbar-caption">Импорт учебных данных и документы</span>
      </header>

      <div className="workspace">
        <aside className="sidebar" aria-label="Навигация по данным">
          <p className="sidebar-heading">Разделы</p>
          <label className="section-search">
            <span className="search-icon" aria-hidden="true">⌕</span>
            <span className="sr-only">Поиск по разделам</span>
            <input
              type="search"
              value={sectionSearch}
              onChange={(event) => setSectionSearch(event.target.value)}
              placeholder="Найти раздел"
              autoComplete="off"
            />
            {sectionSearch && <button type="button" className="section-search-clear" aria-label="Очистить поиск по разделам" onClick={() => setSectionSearch('')}>×</button>}
          </label>
          {visibleGroups.length === 0 && <p className="nav-empty" role="status">Разделы не найдены</p>}
          {visibleGroups.map((group) => (
            <details className="nav-group" key={group.id} open>
              <summary>{group.label}</summary>
              <nav aria-label={group.label}>
                {group.resources.map((id) => (
                  <a key={id} href={`/${RESOURCES[id].route}`} className={`nav-link${resourceId === id ? ' active' : ''}`} aria-current={resourceId === id ? 'page' : undefined} onClick={(event) => navigate(event, id)}>{RESOURCES[id].label}</a>
                ))}
              </nav>
            </details>
          ))}
        </aside>

        <section className="content-card" aria-labelledby="page-title">
          <div className="page-heading">
            <div>
              <p className="eyebrow">IThub · {groupLabel(resource.group)}</p>
              <h1 id="page-title">{resource.label}</h1>
              <p className="intro">{resource.description}</p>
            </div>
            <button className="primary-action" type="button" onClick={() => setDialog({ mode: 'edit', item: null, error: '' })}>
              <span aria-hidden="true">＋</span> Добавить {resource.singular}
            </button>
          </div>

          <div className="list-toolbar">
            <label className="search-form">
              <span className="search-icon" aria-hidden="true">⌕</span>
              <span className="sr-only">Поиск в разделе {resource.label}</span>
              <input value={search} onChange={(event) => setSearch(event.target.value)} placeholder={`Поиск: ${resource.label.toLocaleLowerCase('ru')}`} />
            </label>
            <span className="record-count">{loading ? 'Загружаем…' : `${visibleRows.length} записей`}</span>
          </div>

          {filters.length > 0 && <form className="filters-panel" onSubmit={applyFilters}>
            <div className="filters-heading">
              <strong>Фильтры</strong>
              {filterOptionsLoading && <span className="filter-loading"><span className="loader" />Загружаем варианты</span>}
            </div>
            <div className="filter-fields">
              {filters.map((field) => <FilterField
                key={field.key}
                field={field}
                value={filterValues[field.key] ?? ''}
                options={filterOptions}
                onChange={(value) => setFilterValues((current) => ({ ...current, [field.key]: value }))}
              />)}
            </div>
            <div className="filter-actions">
              <button className="primary-action filter-apply" type="submit">Применить</button>
              {hasAppliedFilters && <button className="filter-reset" type="button" onClick={clearFilters}>Сбросить фильтры</button>}
            </div>
          </form>}

          {pageError && <div className="notice notice-error" role="alert"><strong>Не удалось загрузить данные</strong><span>{pageError}</span><button type="button" className="text-button" onClick={() => loadRows(resourceId)}>Повторить</button></div>}

          <div className="table-wrap">
            <table>
              <thead><tr><th className="id-cell">ID</th>{resource.columns.map(([key, label]) => <th key={key}>{label}</th>)}<th className="actions-heading">Действия</th></tr></thead>
              <tbody>
                {loading ? <tr><td className="table-state" colSpan={resource.columns.length + 2}><span className="loader" />Загружаем записи…</td></tr>
                  : visibleRows.length === 0 ? <tr><td className="table-state empty-state" colSpan={resource.columns.length + 2}><span className="empty-icon" aria-hidden="true">⌕</span><strong>{pageError ? 'Список недоступен' : 'Записей пока нет'}</strong><span>{pageError ? 'Проверьте доступность API и попробуйте загрузить список ещё раз.' : search ? 'По вашему запросу ничего не найдено.' : `Добавьте первую запись в разделе «${resource.label}».`}</span></td></tr>
                    : visibleRows.map((row) => <tr key={row.id}>
                      <td className="muted-cell">{row.id}</td>
                      {resource.columns.map(([key]) => <td key={key}>{displayValue(row, key)}</td>)}
                      <td><RowActions onEdit={() => setDialog({ mode: 'edit', item: row, error: '' })} onDelete={() => setDialog({ mode: 'delete', item: row, error: '' })} /></td>
                    </tr>)}
              </tbody>
            </table>
          </div>
          <p className="table-footnote">Доступно записей: {rows.length}. Данные загружаются из API и сохраняются в базе проекта.</p>
        </section>
      </div>

      {dialog && <div className="dialog-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget && !saving) setDialog(null); }}>
        <section className="dialog" role="dialog" aria-modal="true" aria-labelledby="dialog-title">
          {dialog.mode === 'delete' ? <>
            <div className="dialog-icon danger-icon" aria-hidden="true">!</div>
            <h2 id="dialog-title">Удалить запись?</h2>
            <p className="dialog-copy">Запись «{recordLabel(dialog.item, resource)}» будет удалена. Это действие нельзя отменить.</p>
            {dialog.error && <div className="notice notice-error" role="alert">{dialog.error}</div>}
            <div className="dialog-actions"><button className="secondary-action" type="button" disabled={saving} onClick={() => setDialog(null)}>Отмена</button><button className="danger-action" type="button" disabled={saving} onClick={deleteItem}>{saving ? 'Удаляем…' : 'Удалить'}</button></div>
          </> : <>
            <p className="eyebrow">IThub · {resource.label.toLocaleLowerCase('ru')}</p>
            <h2 id="dialog-title">{dialog.item ? 'Редактировать запись' : `Новая запись: ${resource.singular}`}</h2>
            <p className="dialog-copy">Заполните поля и сохраните запись в разделе «{resource.label}».</p>
            {dialog.optionsError && <div className="notice notice-error" role="alert">Не удалось загрузить варианты связанных записей: {dialog.optionsError}</div>}
            {optionsLoading && <p className="options-loading"><span className="loader" />Загружаем варианты для связанных полей…</p>}
            <form className="edit-form" onSubmit={submitForm}>
              <ResourceFields fields={dialog.item ? (resource.editFields || resource.fields) : (resource.createFields || resource.fields)} item={dialog.item} options={options} />
              {dialog.error && <div className="notice notice-error" role="alert">{dialog.error}</div>}
              <div className="dialog-actions"><button className="secondary-action" type="button" disabled={saving} onClick={() => setDialog(null)}>Отмена</button><button className="primary-action" type="submit" disabled={saving}>{saving ? 'Сохраняем…' : 'Сохранить'}</button></div>
            </form>
          </>}
        </section>
      </div>}
    </main>
  );
}

function FilterField({ field, value, options, onChange }) {
  const control = field.type === 'relation' || field.type === 'choice'
    ? <select value={value} onChange={(event) => onChange(event.target.value)}>
      <option value="">Все значения</option>
      {field.type === 'relation'
        ? (options[field.source] || []).map((row) => <option key={row.id} value={row.id}>{optionLabel(row, field.source)}</option>)
        : field.options.map(([key, title]) => <option key={key} value={key}>{title}</option>)}
    </select>
    : <input
      type={field.type === 'number' || field.type === 'date' ? field.type : 'search'}
      step={field.type === 'number' ? '1' : undefined}
      value={value}
      onChange={(event) => onChange(event.target.value)}
      placeholder="Любое значение"
    />;
  return <label className="filter-field"><span>{field.label}</span>{control}</label>;
}

function ResourceFields({ fields, item, options }) {
  const groups = Object.groupBy ? Object.groupBy(fields, (field) => field.group || 'Данные') : fields.reduce((result, field) => {
    const key = field.group || 'Данные'; (result[key] ||= []).push(field); return result;
  }, {});
  return <div className="resource-fields">
    {Object.entries(groups).map(([group, items]) => <fieldset className="field-group" key={group}>
      {Object.keys(groups).length > 1 && <legend>{group}</legend>}
      <div className="field-grid">{items.map((field, index) => <Field key={field.key} field={field} item={item} options={options} autoFocus={index === 0 && group === Object.keys(groups)[0]} />)}</div>
    </fieldset>)}
  </div>;
}

function Field({ field, item, options, autoFocus }) {
  const value = item?.[field.key];
  const label = <span>{field.label}{field.required && <> <b>*</b></>}</span>;
  const common = { name: field.key, required: field.required, autoFocus, defaultValue: field.type === 'date' && value ? String(value).slice(0, 10) : value ?? '' };
  if (field.type === 'relation') {
    const choices = options[field.source] || [];
    return <label className="field">{label}<select {...common}><option value="">{field.required ? 'Выберите запись' : 'Не выбрано'}</option>{choices.map((row) => <option key={row.id} value={row.id}>{optionLabel(row, field.source)}</option>)}</select>{choices.length === 0 && <small>Сначала добавьте связанные записи в разделе «{RESOURCES[field.source].label}».</small>}</label>;
  }
  if (field.type === 'choice') return <label className="field">{label}<select {...common}><option value="">Выберите значение</option>{field.options.map(([key, title]) => <option key={key} value={key}>{title}</option>)}</select></label>;
  if (field.type === 'textarea') return <label className="field">{label}<textarea {...common} rows="2" /></label>;
  const type = field.type === 'number' || field.type === 'date' ? field.type : field.key === 'email' ? 'email' : 'text';
  return <label className="field">{label}<input {...common} type={type} step={field.type === 'number' ? '1' : undefined} /></label>;
}

function RowActions({ onEdit, onDelete }) {
  return <div className="row-actions"><button className="icon-action" type="button" onClick={onEdit} aria-label="Редактировать" title="Редактировать">✎</button><button className="icon-action delete-icon" type="button" onClick={onDelete} aria-label="Удалить" title="Удалить">⌫</button></div>;
}

function parseField(value, field) {
  if (value === '' || value === null) return field.required ? '' : null;
  if (field.type === 'number' || field.type === 'relation' || field.type === 'choice') return Number(value);
  if (field.type === 'date') return `${value}T00:00:00`;
  return String(value).trim();
}

function groupLabel(id) {
  return RESOURCE_GROUPS.find((group) => group.id === id)?.label.toLocaleLowerCase('ru') || 'данные';
}

function recordLabel(row, resource) {
  const keys = resource.columns.map(([key]) => key).filter((key) => !key.toLowerCase().endsWith('id'));
  return keys.map((key) => row?.[key]).find((value) => value !== null && value !== undefined && value !== '') || `ID ${row?.id}`;
}

export default App;
