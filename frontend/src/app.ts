interface Sentence {
  id: number;
  text: string;
  language: string;
  source: string;
  notes: string;
  tags: string[];
  isFavorite: boolean;
}

interface SentenceForm {
  text: string;
  language: string;
  source: string;
  notes: string;
  tags: string;
}

class SentenceController {
  public sentences: Sentence[] = [];
  public keyword = "";
  public tag = "";
  public favoritesOnly = false;
  public form: SentenceForm = this.emptyForm();

  public static $inject = ["$http"];

  public constructor(private readonly $http: angular.IHttpService) {
    this.load();
  }

  public async load(): Promise<void> {
    const query = `
      query SearchSentences($keyword: String, $tag: String, $favoritesOnly: Boolean) {
        sentences(keyword: $keyword, tag: $tag, favoritesOnly: $favoritesOnly) {
          id
          text
          language
          source
          notes
          tags
          isFavorite
        }
      }
    `;

    const response = await this.graphql<{ sentences: Sentence[] }>(query, {
      keyword: this.keyword,
      tag: this.tag,
      favoritesOnly: this.favoritesOnly
    });

    this.sentences = response.sentences;
  }

  public async create(): Promise<void> {
    const mutation = `
      mutation CreateSentence($input: SentenceInput!) {
        createSentence(input: $input) {
          id
        }
      }
    `;

    await this.graphql(mutation, {
      input: {
        text: this.form.text,
        language: this.form.language,
        source: this.form.source,
        notes: this.form.notes,
        tags: this.form.tags
          .split(",")
          .map(tag => tag.trim())
          .filter(tag => tag.length > 0)
      }
    });

    this.form = this.emptyForm();
    await this.load();
  }

  public async toggleFavorite(sentence: Sentence): Promise<void> {
    const mutation = `
      mutation ToggleFavorite($id: Int!) {
        toggleFavorite(id: $id) {
          id
          isFavorite
        }
      }
    `;

    await this.graphql(mutation, { id: sentence.id });
    await this.load();
  }

  public async delete(sentence: Sentence): Promise<void> {
    const mutation = `
      mutation DeleteSentence($id: Int!) {
        deleteSentence(id: $id)
      }
    `;

    await this.graphql(mutation, { id: sentence.id });
    await this.load();
  }

  private async graphql<T>(query: string, variables?: Record<string, unknown>): Promise<T> {
    const response = await this.$http.post<{ data?: T; errors?: unknown[] }>(
      "http://localhost:5000/graphql/",
      { query, variables }
    );

    if (response.data.errors) {
      throw new Error(JSON.stringify(response.data.errors));
    }

    return response.data.data as T;
  }

  private emptyForm(): SentenceForm {
    return {
      text: "",
      language: "",
      source: "",
      notes: "",
      tags: ""
    };
  }
}

angular
  .module("sentenceStock", [])
  .controller("SentenceController", SentenceController);
