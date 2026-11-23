import { useCallback, useEffect, useMemo, useState } from 'react';

const sections = {
  students: {
    title: 'Студенты',
    description: 'Управляйте данными студентов и информацией о поступлении.',
    endpoint: '/api/students',
  },
  organizations: {
    title: 'Организации',
    description: 'Просматривайте и редактируйте организации в системе.',
    endpoint: '/api/organizations',
  },
};

async function request(url, options) {
  const response = await fetch(url, options);
  const text = await response.text();
  let payload = null;
  if (text) {
    try { payload = JSON.parse(text); } catch { payload = text; }
  }
  if (!response.ok) {
    const validation = payload?.errors && Object.values(payload.errors).flat().join(' ');
    const message = Array.isArray(payload)
      ? payload.join(' ')
      : payload?.detail || payload?.title || validation || payload?.message || payload || `Ошибка запроса (${response.status})`;
    throw new Error(String(message));
  }
  return payload;
}

function App() {
  const [section, setSection] = useState(() => location.pathname.startsWith('/organizations') ? 'organizations' : 'students');
  const [rows, setRows] = useState([]);
  const [loading, setLoading] = useState(true);
  const [pageError, setPageError] = useState('');
  const [search, setSearch] = useState('');
  const [organizationQuery, setOrganizationQuery] = useState('');
  const [dialog, setDialog] = useState(null);
  const [saving, setSaving] = useState(false);

  const loadRows = useCallback(async (activeSection = section, query = '') => {
    setLoading(true);
    setPageError('');
    try {
      const suffix = activeSection === 'organizations' && query.trim()
        ? `?name=${encodeURIComponent(query.trim())}`
        : '';
      const data = await request(`${sections[activeSection].endpoint}${suffix}`);
      setRows(Array.isArray(data) ? data : []);
    } catch (error) {
      setPageError(error.message || 'Не удалось загрузить данные. Проверьте подключение к API.');
      setRows([]);
    } finally {
      setLoading(false);
    }
  }, [section]);

  useEffect(() => {
    const onPopState = () => {
      const next = location.pathname.startsWith('/organizations') ? 'organizations' : 'students';
      setSection(next);
      setSearch('');
      setOrganizationQuery('');
    };
    addEventListener('popstate', onPopState);
    loadRows(section, '');
    return () => removeEventListener('popstate', onPopState);
  }, [section, loadRows]);

  useEffect(() => {
    if (!dialog) return undefined;
    const onKeyDown = (event) => {
      if (event.key === 'Escape' && !saving) setDialog(null);
    };
    addEventListener('keydown', onKeyDown);
    return () => removeEventListener('keydown', onKeyDown);
  }, [dialog, saving]);

  function navigate(event, next) {
    event.preventDefault();
    if (next === section) return;
    history.pushState({}, '', next === 'students' ? '/students' : '/organizations');
    setSection(next);
    setSearch('');
    setOrganizationQuery('');
  }

  const visibleRows = useMemo(() => {
    if (section !== 'students' || !search.trim()) return rows;
    const needle = search.trim().toLocaleLowerCase('ru');
    return rows.filter((student) => [student.firstName, student.lastName, student.patronymic, student.email, student.id]
      .filter(Boolean).join(' ').toLocaleLowerCase('ru').includes(needle));
  }, [rows, search, section]);

  async function submitForm(event) {
    event.preventDefault();
    const form = event.currentTarget;
    const values = new FormData(form);
    const payload = section === 'students'
      ? {
          email: String(values.get('email')).trim(),
          firstName: String(values.get('firstName')).trim(),
          lastName: String(values.get('lastName')).trim(),
          patronymic: String(values.get('patronymic')).trim() || null,
          admissionBase: values.get('admissionBase') ? Number(values.get('admissionBase')) : null,
          admissionYear: values.get('admissionYear') ? Number(values.get('admissionYear')) : null,
        }
      : { name: String(values.get('name')).trim() };

    setSaving(true);
    setDialog({ ...dialog, error: '' });
    try {
      const editing = Boolean(dialog.item);
      const endpoint = `${sections[section].endpoint}${editing ? `/${dialog.item.id}` : ''}`;
      await request(endpoint, {
        method: editing ? 'PUT' : 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });
      setDialog(null);
      await loadRows(section, section === 'organizations' ? organizationQuery : '');
    } catch (error) {
      setDialog((current) => ({ ...current, error: error.message || 'Не удалось сохранить запись.' }));
    } finally {
      setSaving(false);
    }
  }

  async function deleteItem() {
    setSaving(true);
    try {
      await request(`${sections[section].endpoint}/${dialog.item.id}`, { method: 'DELETE' });
      setDialog(null);
      await loadRows(section, section === 'organizations' ? organizationQuery : '');
    } catch (error) {
      setDialog((current) => ({ ...current, error: error.message || 'Не удалось удалить запись.' }));
    } finally {
      setSaving(false);
    }
  }

  function submitSearch(event) {
    event.preventDefault();
    setOrganizationQuery(search);
    loadRows(section, search);
  }

  const activeSection = sections[section];

  return (
    <main className="app-shell">
      <header className="topbar">
        <a className="brand" href="/students" onClick={(event) => navigate(event, 'students')} aria-label="IThub, студенты">
          <span className="brand-mark">i</span><span>IThub</span>
        </a>
        <nav className="main-nav" aria-label="Разделы">
          <a className={section === 'students' ? 'active' : ''} href="/students" onClick={(event) => navigate(event, 'students')}>Студенты</a>
          <a className={section === 'organizations' ? 'active' : ''} href="/organizations" onClick={(event) => navigate(event, 'organizations')}>Организации</a>
        </nav>
        <span className="topbar-caption">Учебные данные</span>
      </header>

      <section className="content-card" aria-labelledby="page-title">
        <div className="page-heading">
          <div>
            <p className="eyebrow">IThub · {section === 'students' ? 'справочники' : 'организации'}</p>
            <h1 id="page-title">{activeSection.title}</h1>
            <p className="intro">{activeSection.description}</p>
          </div>
          <button className="primary-action" type="button" onClick={() => setDialog({ mode: 'edit', item: null })}>
            <span aria-hidden="true">＋</span> Добавить {section === 'students' ? 'студента' : 'организацию'}
          </button>
        </div>

        <div className="list-toolbar">
          <form className="search-form" onSubmit={submitSearch} role="search">
            <label className="sr-only" htmlFor="search">{section === 'students' ? 'Поиск студентов' : 'Поиск организаций'}</label>
            <span className="search-icon" aria-hidden="true">⌕</span>
            <input id="search" value={search} onChange={(event) => setSearch(event.target.value)} placeholder={section === 'students' ? 'Имя, фамилия или email' : 'Название организации'} />
            {section === 'organizations' && <button className="search-submit" type="submit">Найти</button>}
          </form>
          <span className="record-count">{loading ? 'Загружаем…' : `${visibleRows.length} ${pluralize(visibleRows.length, section)}`}</span>
        </div>

        {pageError && <div className="notice notice-error" role="alert"><strong>Не удалось загрузить данные</strong><span>{pageError}</span><button type="button" className="text-button" onClick={() => loadRows(section, organizationQuery)}>Повторить</button></div>}

        <div className="table-wrap">
          <table>
            <thead>
              {section === 'students' ? (
                <tr><th className="id-cell">ID</th><th>Студент</th><th>Email</th><th>Поступление</th><th className="actions-heading">Действия</th></tr>
              ) : (
                <tr><th className="id-cell">ID</th><th>Название организации</th><th className="actions-heading">Действия</th></tr>
              )}
            </thead>
            <tbody>
              {loading ? <tr><td className="table-state" colSpan={section === 'students' ? 5 : 3}><span className="loader" />Загружаем записи…</td></tr>
                : visibleRows.length === 0 ? <tr><td className="table-state empty-state" colSpan={section === 'students' ? 5 : 3}><span className="empty-icon" aria-hidden="true">⌕</span><strong>{pageError ? 'Список недоступен' : 'Записей пока нет'}</strong><span>{pageError ? 'Попробуйте загрузить данные ещё раз.' : search ? 'По вашему запросу ничего не найдено.' : 'Добавьте первую запись, чтобы она появилась в этом списке.'}</span></td></tr>
                  : visibleRows.map((row) => section === 'students' ? (
                    <tr key={row.id}>
                      <td className="muted-cell">{row.id}</td>
                      <td><div className="person-cell"><span className="avatar">{initials(row)}</span><span className="person-name">{[row.lastName, row.firstName, row.patronymic].filter(Boolean).join(' ')}</span></div></td>
                      <td><a className="email-link" href={`mailto:${row.email}`}>{row.email}</a></td>
                      <td>{row.admissionBase || row.admissionYear ? <span className="admission-tag">{row.admissionBase ? `После ${row.admissionBase} класса` : 'База не указана'}{row.admissionYear ? ` · ${row.admissionYear}` : ''}</span> : <span className="muted-cell">Не указано</span>}</td>
                      <td><RowActions onEdit={() => setDialog({ mode: 'edit', item: row })} onDelete={() => setDialog({ mode: 'delete', item: row })} /></td>
                    </tr>
                  ) : (
                    <tr key={row.id}>
                      <td className="muted-cell">{row.id}</td>
                      <td><div className="organization-cell"><span className="organization-mark" aria-hidden="true">⌂</span><span className="person-name">{row.name}</span></div></td>
                      <td><RowActions onEdit={() => setDialog({ mode: 'edit', item: row })} onDelete={() => setDialog({ mode: 'delete', item: row })} /></td>
                    </tr>
                  ))}
            </tbody>
          </table>
        </div>
        <p className="table-footnote">Изменения сохраняются в базе данных сразу после подтверждения.</p>
      </section>

      <footer className="app-footer"><span>© IThub</span><span>Управление учебными данными</span></footer>

      {dialog && <div className="dialog-backdrop" role="presentation" onMouseDown={(event) => { if (event.target === event.currentTarget && !saving) setDialog(null); }}>
        <section className="dialog" role="dialog" aria-modal="true" aria-labelledby="dialog-title">
          {dialog.mode === 'delete' ? <>
            <div className="dialog-icon danger-icon" aria-hidden="true">!</div>
            <h2 id="dialog-title">Удалить {section === 'students' ? 'студента' : 'организацию'}?</h2>
            <p className="dialog-copy">{section === 'students' ? `${[dialog.item.lastName, dialog.item.firstName, dialog.item.patronymic].filter(Boolean).join(' ')} будет удалён из списка.` : `«${dialog.item.name}» будет удалена из списка.`} Это действие нельзя отменить.</p>
            {dialog.error && <div className="notice notice-error" role="alert">{dialog.error}</div>}
            <div className="dialog-actions"><button className="secondary-action" type="button" disabled={saving} onClick={() => setDialog(null)}>Отмена</button><button className="danger-action" type="button" disabled={saving} onClick={deleteItem}>{saving ? 'Удаляем…' : 'Удалить'}</button></div>
          </> : <>
            <p className="eyebrow">IThub · {section === 'students' ? 'студенты' : 'организации'}</p>
            <h2 id="dialog-title">{dialog.item ? 'Редактировать запись' : `Новая ${section === 'students' ? 'запись' : 'организация'}`}</h2>
            <p className="dialog-copy">{dialog.item ? 'Обновите необходимые поля и сохраните изменения.' : 'Заполните данные. Обязательные поля отмечены звёздочкой.'}</p>
            <form className="edit-form" onSubmit={submitForm}>
              {section === 'students' ? <StudentFields item={dialog.item} /> : <label className="field"><span>Название организации <b>*</b></span><input name="name" defaultValue={dialog.item?.name || ''} maxLength="200" autoFocus required /></label>}
              {dialog.error && <div className="notice notice-error" role="alert">{dialog.error}</div>}
              <div className="dialog-actions"><button className="secondary-action" type="button" disabled={saving} onClick={() => setDialog(null)}>Отмена</button><button className="primary-action" type="submit" disabled={saving}>{saving ? 'Сохраняем…' : 'Сохранить'}</button></div>
            </form>
          </>}
        </section>
      </div>}
    </main>
  );
}

function StudentFields({ item }) {
  return <div className="student-fields">
    <label className="field field-wide"><span>Email <b>*</b></span><input type="email" name="email" defaultValue={item?.email || ''} maxLength="320" placeholder="student@example.com" autoFocus required /></label>
    <label className="field"><span>Фамилия <b>*</b></span><input name="lastName" defaultValue={item?.lastName || ''} maxLength="200" required /></label>
    <label className="field"><span>Имя <b>*</b></span><input name="firstName" defaultValue={item?.firstName || ''} maxLength="200" required /></label>
    <label className="field field-wide"><span>Отчество</span><input name="patronymic" defaultValue={item?.patronymic || ''} maxLength="200" /></label>
    <label className="field"><span>База поступления</span><select name="admissionBase" defaultValue={item?.admissionBase || ''}><option value="">Не указана</option><option value="9">После 9 класса</option><option value="11">После 11 класса</option></select></label>
    <label className="field"><span>Год поступления</span><input type="number" name="admissionYear" min="1900" max="2200" placeholder="Например, 2025" defaultValue={item?.admissionYear || ''} /></label>
  </div>;
}

function RowActions({ onEdit, onDelete }) {
  return <div className="row-actions"><button className="icon-action" type="button" onClick={onEdit} aria-label="Редактировать" title="Редактировать">✎</button><button className="icon-action delete-icon" type="button" onClick={onDelete} aria-label="Удалить" title="Удалить">⌫</button></div>;
}

function initials(student) {
  return `${student.firstName?.[0] || ''}${student.lastName?.[0] || ''}`.toLocaleUpperCase('ru');
}

function pluralize(count, section) {
  if (section === 'students') {
    const mod10 = count % 10; const mod100 = count % 100;
    return mod10 === 1 && mod100 !== 11 ? 'студент' : mod10 >= 2 && mod10 <= 4 && (mod100 < 12 || mod100 > 14) ? 'студента' : 'студентов';
  }
  const mod10 = count % 10; const mod100 = count % 100;
  return mod10 === 1 && mod100 !== 11 ? 'организация' : mod10 >= 2 && mod10 <= 4 && (mod100 < 12 || mod100 > 14) ? 'организации' : 'организаций';
}

export default App;
