import { EmarketingTemplatePage } from './app.po';

describe('Emarketing App', function() {
  let page: EmarketingTemplatePage;

  beforeEach(() => {
    page = new EmarketingTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
